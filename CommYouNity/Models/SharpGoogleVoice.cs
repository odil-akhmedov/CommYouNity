/*

Copyright (C) 2012 Alex Yumashev
Jitbit Software
http://www.jitbit.com/
https://bitbucket.org/jitbit/sharpgooglevoice/

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CommYouNity.Models
{
	class SharpGoogleVoice
	{
		private CookieWebClient _webClient;

		private string _rnrse;

		private string _username;
		private string _password;

		private const string USERAGENT = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 2_2_1 like Mac OS X; en-us) AppleWebKit/525.18.1 (KHTML, like Gecko) Version/3.1.1 Mobile/5H11 Safari/525.20";

		public SharpGoogleVoice(string username, string password)
		{
			_username = username;
			_password = password;
			_webClient = new CookieWebClient();
		}

		private void Login()
		{
			_webClient.Headers.Add("User-agent", USERAGENT); //mobile user agent to save bandwidth (google will serve mobile version of the page)

			//get "GALX" field value from google login page
			string response = Encoding.ASCII.GetString(_webClient.DownloadData("https://accounts.google.com/ServiceLogin?service=grandcentral"));
			Regex galxRegex = new Regex(@"name=""GALX"".*?value=""(.*?)""", RegexOptions.Singleline);
			string galx = galxRegex.Match(response).Groups[1].Value;

			//sending login info
			_webClient.Headers.Add("Content-type", "application/x-www-form-urlencoded;charset=utf-8");
			_webClient.Headers.Add("User-agent", USERAGENT); //mobile user agent to save bandwidth (google will serve mobile version of the page)
			byte[] responseArray = _webClient.UploadData(
				"https://accounts.google.com/ServiceLogin?service=grandcentral",
				"POST",
				PostParameters(new Dictionary<string, string>
				               	{
				               		{"Email", _username},
				               		{"Passwd", _password},
				               		{"GALX", galx},
									{"PersistentCookie", "yes"}
				               	}));
			response = Encoding.ASCII.GetString(responseArray);
		}

		/// <summary>
		/// creates a byte-array ready to be sent with a POST-request
		/// </summary>
		private static byte[] PostParameters(IDictionary<string, string> parameters)
		{
			string paramStr = "";

			foreach (string key in parameters.Keys)
			{
				paramStr += key + "=" + HttpUtility.UrlEncode(parameters[key]) + "&";
			}

			return Encoding.ASCII.GetBytes(paramStr);
		}

		private void TryGetRNRSE()
		{
			if (!GetRNRSE())
			{
				//we can't find the required field on the page. Probably we're logged out. Let's try to login
				Login();
				if (!GetRNRSE())
				{
					throw new Exception("Unable to get the Session-id field from Google.");
				}
			}
		}

		/// <summary>
		/// Gets google's "session id" hidden-field value
		/// </summary>
		private bool GetRNRSE()
		{
			_webClient.Headers.Add("User-agent", USERAGENT); //mobile user agent to save bandwidth (google will serve mobile version of the page)

			//get goovle voice "homepage" (mobile version - to save bandwidth)
			string response = Encoding.ASCII.GetString(_webClient.DownloadData("https://www.google.com/voice/m"));

			//find the hidden field
			Regex rnrRegex = new Regex(@"<input.*?name=""_rnr_se"".*?value=""(.*?)""");
			if (rnrRegex.IsMatch(response))
			{
				_rnrse = rnrRegex.Match(response).Groups[1].Value;
				return true;
			}
			return false;
		}


		/// <summary>
		/// Send s text-message to teh specified number
		/// </summary>
		/// <param name="number">Phone number in '+1234567890'-format </param>
		/// <param name="text">Message text</param>
		public void SendSMS(string number, string text)
		{
			if (!ValidateNumber(number))
				throw new FormatException("Wrong number format. Should be '+1234567890'. Please try again.");

			TryGetRNRSE();

			text = text.Length <= 160 ? text : text.Substring(0, 160);

			byte[] parameters = PostParameters(new Dictionary<string, string>
			                                   	{
			                                   		{"phoneNumber", number},
			                                   		{"text", text},
			                                   		{"_rnr_se", _rnrse}
			                                   	});

			_webClient.Headers.Add("Content-type", "application/x-www-form-urlencoded;charset=utf-8");
			byte[] responseArray = _webClient.UploadData("https://www.google.com/voice/sms/send", "POST", parameters);
			string response = Encoding.ASCII.GetString(responseArray);

			if (response.IndexOf("\"ok\":true") == -1)
				throw new Exception("Google did not answer with an OK response\n\n"+response);
		}

		public static bool ValidateNumber(string number)
		{
			if (number == null) return false;
			return Regex.IsMatch(number, @"^\+\d{11}$");
		}
	}

	internal class CookieWebClient : System.Net.WebClient
	{
		private CookieContainer _cookieContainer = new CookieContainer();
		public CookieContainer CookieContainer
		{
			get { return _cookieContainer; }
			set { _cookieContainer = value; }
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest request = base.GetWebRequest(address);
			if (request is HttpWebRequest)
			{
				(request as HttpWebRequest).CookieContainer = _cookieContainer;
			}
			return request;
		}
	}
}
