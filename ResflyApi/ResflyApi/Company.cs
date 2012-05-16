// 
// Company.cs
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
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Resfly
{
    [DataContract]
    public class Company
    {   
        private ResflyApi ResflyApi;
        
        [DataMember(Name = "id")]
        public int Id { get; set; }
        
        [DataMember(Name = "date_created")]
        public string dateCreatedString { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "type")]
        public string Type { get; set; }
                
        [DataMember(Name = "url")]
        public string Url { get; set; }
        
        [DataMember(Name = "job_slots")]
        public int JobSlots { get; set; }
        
        public Company(ResflyApi resflyApi)
        {
            this.ResflyApi = resflyApi;
        }
        
        public bool Save()
        {
            Response response;
            
            if (this.Id < 1)
            {
                response = this.ResflyApi.MakeRequest(
                    "/companies", 
                    "POST", 
                    this.ResflyApi.SerializeToJson(this)
                );
            
                if (response.HttpWebResponse.StatusCode != HttpStatusCode.Created)
                {
                    return false;
                }               
            }
            else
            {
                response = this.ResflyApi.MakeRequest(
                    "/companies/" + this.Id, 
                    "PUT", 
                    this.ResflyApi.SerializeToJson(this)
                );
            }           
            
            Company company = response.Company;
            this.Id = company.Id;
            this.DateCreated = DateTime.Parse(company.dateCreatedString);
            this.Name = company.Name;
            this.Type = company.Type;
            this.Url = company.Url;
            this.JobSlots = company.JobSlots;
            
            return true;
        }
        
        public bool Delete()
        {
            if (this.Id < 1)
            {
                return false;
            }
            
            Response response = this.ResflyApi.MakeRequest(
                "/companies/" + this.Id,
                "DELETE"
            );
            
            if (response.HttpWebResponse.StatusCode != HttpStatusCode.NoContent)
            {
                return false;
            }
            
            return true;
        }
        
        public List<Job> GetJobs()
        {
            Response response = this.ResflyApi.MakeRequest(
                "/companies/" + this.Id + "/jobs",
                "GET"
            );
            
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
    }
}

