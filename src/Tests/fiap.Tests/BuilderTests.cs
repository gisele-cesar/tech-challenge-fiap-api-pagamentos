﻿using fiap.Application;
using fiap.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace fiap.Tests
{
    public class BuilderTests
    {
         
        

        [Fact]
        public void AddModule_Test()
        {
            var _serviceCollection = new ServiceCollection();

            _serviceCollection.AddApplicationModule();
            _serviceCollection.AddServicesModule();

            var _serviceProvider = _serviceCollection.BuildServiceProvider();


            Assert.NotNull(_serviceProvider);

        }
    }
}
