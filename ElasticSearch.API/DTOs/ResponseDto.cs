﻿using System.Net;

namespace ElasticSearch.API.DTOs
{
    public record ResponseDto<T>
    {
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public HttpStatusCode Status { get; set; }

        public static ResponseDto<T> Success(T data, HttpStatusCode status)
        {
            return new ResponseDto<T> { Data = data, Status = status };
        }

        public static ResponseDto<T>Fail(List<string> errors,HttpStatusCode status) {
        return new ResponseDto<T> { Status = status, Errors = errors };
        }
    }
}
