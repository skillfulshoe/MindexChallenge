﻿using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetReportingStructureById_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                FirstName = "George",
                LastName = "Harrison",
                DirectReports = null
            };
            
            var reportingStructure = new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = 0
            };


            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employee.EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var newReportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(reportingStructure.Employee.EmployeeId, newReportingStructure.Employee.EmployeeId);
            Assert.AreEqual(reportingStructure.Employee.FirstName, newReportingStructure.Employee.FirstName);
            Assert.AreEqual(reportingStructure.Employee.LastName, newReportingStructure.Employee.LastName);
            Assert.AreEqual(reportingStructure.NumberOfReports, newReportingStructure.NumberOfReports);
        }
    }
}
