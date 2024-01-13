# About this repository

In this repository, Felipe will exploring some ways to implement Authentication and Authorization mechanism in ASP.NET
Core to build an ASP.NET Web Application or ASP.NET API Application. I will also be using this repository for my own
reference in future projects I will be working. You can use this code anyway you want, just leave a reference where you
get it :) like I am doing here.

## Everyone in the same page

`Authentication` is a process of verifying and identifying users to allow them access to protected resources. Once a
user is authenticated, an extra step is the `Authorization` which determines whether the user is permitted or denied
access to specific resources. Some application can also contains resources or data that does not requires authentication
and we call this as public access or in some technical `.NET` terms, `Anonymous` access.

In essence, `Authentication` establishes the user's identity, while _Authorization_ check their access to the features
or data within an application.

## Examples

Every project in the Solutions explores an authentication mechanism and to make sure we understand everything. It is
important to mention I started it from the scratch without frameworks and then evolve towards more sophisticated
solutions.

### RawCookieAuthentication

This example illustrates how we could implement an _Authentication_ mechanism based on the existing implementation
available in the ASP.NET Core, but accessing `http` feature via `HtppContext`, without any framework. This
implementation uses cookies to create the state of the authentication within the browser. 

**Warning**: It is not recommend to be used as production code. Please consider ASP.NET Core features to implement it. 