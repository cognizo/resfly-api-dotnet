// 
// ResflyApi.cs
//  
// Author:
//       Graham Floyd <gfloyd@resfly.com>
// 
// Copyright (c) 2012 Resfly, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Resfly
{
    public class ResflyApi
    {       
        public string Url { get; set; }
        
        public string ApiKey { get; set; }
        
        public ResflyApi(string url, string apiKey)
        {
            this.Url = url;
            this.ApiKey = apiKey;
        }
        
        public Response MakeRequest(string uri, string method, string json = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.Url + uri);
            
            request.Accept = "application/json";
            request.Method = method;
            request.Headers.Add("X-Api-Key", this.ApiKey);
            
            switch (method)
            {
                case "POST":
                case "PUT":
                    if (json.Length > 0)
                    {                       
                        request.ContentType = "application/json";
                        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
                
                        request.ContentLength = jsonBytes.Length;
                
                        Stream dataStream = request.GetRequestStream();         
                        dataStream.Write(jsonBytes, 0, jsonBytes.Length);
                        dataStream.Close();
                    }
                    break;
            }
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();              

            Stream stream = response.GetResponseStream();
            string responseString = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                responseString = reader.ReadToEnd();
            }           

            Response apiResponse = new Response();
            if (responseString.Trim().Length > 0)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
                
                MemoryStream responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseString));                     
                object objResponse = serializer.ReadObject(responseStream);
                
                apiResponse = objResponse as Response;              
            }
            
            apiResponse.HttpWebResponse = response;
            apiResponse.ResponseString = responseString;
            
            return apiResponse;
        }
        
        public Company GetCompany(int Id)
        {
            Response response = MakeRequest(
                "/companies/" + Id.ToString(),
                "GET"
            );
            
            return response.Company;
        }
        
        public List<Company> GetCompanies()
        {
            Response response = MakeRequest("/companies", "GET");
            
            List<Company> companies = new List<Company>();      
            
            if (response.Companies != null)
            {               
                foreach (Response companyResponse in response.Companies)
                {
                    companies.Add(companyResponse.Company);
                }
            }
            
            return companies;
        }
        
        public Job GetJob(int Id)
        {
            Response response = MakeRequest("/jobs/" + Id, "GET");
            
            Job job = response.Job;
            job.ResflyApi = this;           
                        
            return job;
        }
        
        public List<Job> GetJobs()
        {
            Response response = MakeRequest("/jobs", "GET");
            
            List<Job> jobs = new List<Job>();
            
            if (response.Jobs != null)
            {
                foreach (Response jobResponse in response.Jobs)
                {
                    jobs.Add(jobResponse.Job);
                }
            }
            
            return jobs;
        }
        
        public Candidate GetCandidate(int Id)
        {
            Response response = MakeRequest("/candidates/" + Id, "GET");
            
            return response.Candidate;
        }
        
        public List<Candidate> GetCandidates()
        {
            Response response = MakeRequest("/candidates", "GET");
            
            List<Candidate> candidates = new List<Candidate>();
            
            if (response.Candidates != null)
            {
                foreach (Response candidateResponse in response.Candidates)
                {
                    candidates.Add(candidateResponse.Candidate);
                }
            }
            
            return candidates;
        }
        
        public string SerializeToJson(object obj)
        {           
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            
            serializer.WriteObject(stream, obj);
            stream.Position = 0;
            
            string json = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            
            return "{\"" + obj.GetType().Name.ToLower() + "\":" + json + "}";
        }
    }
}

