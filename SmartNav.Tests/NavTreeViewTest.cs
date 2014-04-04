using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SmartNav.Tests
{
    [TestClass]
    public class NavTreeViewTest
    {

        #region Constructor

        [TestMethod]
        public void ctor_with_rootView_must_assign_Root()
        {
            // Arrange		
            var expectedRoot = new Mock<NavRootView>().Object;
            var sut = new NavTreeView(expectedRoot);
            // Act
            var actual = sut.Root;

            // Assert		
            actual.Should().Be(expectedRoot, "Should assign Root Property");
        }

        [TestMethod]
        public void NavTreeView_must_implement_INavTreeViewModel()
        {
            // Arrange		
            
            // Act
            var actual = MakeSut();
            // Assert		
            actual.Should().BeAssignableTo<INavTreeViewModel>();
        }

        private NavTreeView MakeSut(NavRootView root = null)
        {
            root = root ?? new Mock<NavRootView>().Object;
            return new NavTreeView(root);
        }

        #endregion


    }
}