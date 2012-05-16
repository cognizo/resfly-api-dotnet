// 
// Response.cs
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
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Resfly
{
    [DataContract]
    public class Response
    {
        public HttpWebResponse HttpWebResponse { get; set; }
        
        public string ResponseString { get; set; }
        
        [DataMember(Name = "company")]
        public Company Company { get; set; }
        
        [DataMember(Name = "companies")]
        public List<Response> Companies { get; set; }
        
        [DataMember(Name = "job")]
        public Job Job { get; set; }
        
        [DataMember(Name = "jobs")]
        public List<Response> Jobs { get; set; }
        
        [DataMember(Name = "candidate")]
        public Candidate Candidate { get; set; }
        
        [DataMember(Name = "candidates")]
        public List<Response> Candidates { get; set; }
        
        [DataMember(Name = "errors")]
        public List<Response> Errors { get; set; }
        
        public Response()
        {           
        }
    }
}

