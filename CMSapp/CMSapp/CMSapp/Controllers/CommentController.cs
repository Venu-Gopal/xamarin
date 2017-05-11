//Contains methods that communicate with the REST API
//Methods are used to implement CRUD and other operations on Comment Data
using CMSapp.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMSapp.Controllers
{
    class CommentController
    {
        public static void AddComment(Comment comment)
        {
            var request = new RestRequest("/comments/create", Method.POST);
            request.AddParameter("applicationid", comment.applicationid);
            request.AddParameter("commenter", comment.commenter);
            request.AddParameter("comment_by", comment.comment_by);
            request.AddParameter("comment_time", comment.comment_time);
            request.AddParameter("comment", comment.comment);
            IRestResponse response = new RequestController(request).send();
        }
    }
}
