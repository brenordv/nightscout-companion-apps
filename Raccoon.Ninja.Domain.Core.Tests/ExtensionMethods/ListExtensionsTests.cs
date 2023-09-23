﻿using FluentAssertions;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Tests.ExtensionMethods;

public class ListExtensionsTests
{
    [Fact]
    public void HasElements_Should_Return_True_When_List_Has_Elements()
    {
        //Arrange
        var list = new List<string> {"a", "b", "c"};

        //Act
        var result = list.HasElements();

        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void HasElements_Should_Return_False_When_List_Is_Null()
    {
        //Arrange
        List<string> list = null;

        //Act
        var result = list.HasElements();

        //Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void HasElements_Should_Return_False_When_List_Is_Empty()
    {
        //Arrange
        var list = new List<string>();

        //Act
        var result = list.HasElements();

        //Assert
        result.Should().BeFalse();
    }
}