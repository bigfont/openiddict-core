﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NWebsec.Core.Extensions;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Middleware.Middleware
{
    public class XXssMiddleware : MiddlewareBase
    {
        private readonly IXXssProtectionConfiguration _config;
        private readonly HeaderResult _headerResult;

        public XXssMiddleware(RequestDelegate next, XXssProtectionOptions options)
            : base(next)
        {
            _config = options;
            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateXXssProtectionResult(_config);
        }

        internal override void PreInvokeNext(HttpContext owinEnvironment)
        {
            owinEnvironment.GetNWebsecContext().XXssProtection = _config;

            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.Response.Headers[_headerResult.Name] = _headerResult.Value;
            }
        }
    }
}