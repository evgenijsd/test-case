using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using test_case.api.Constants;
using test_case.api.Context;
using test_case.api.Filters;
using test_case.api.Interfaces;
using test_case.api.Middlewares;
using test_case.api.Models.DTO;
using test_case.api.Models.Transaction;
using test_case.api;

var app = Infrastructure.ConfigureApplication(args);

app.Run();
