using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartNav.Tests.NavView;

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
            var expectedRoot = new NavRootView("someId", "someName", new Mock<INavNodeProperties>().Object);
            var sut = new NavTreeView(expectedRoot, new Mock<ViewContext>().Object);
            // Act
            var actual = sut.Root;

            // Assert		
            actual.Should().Be(expectedRoot, "Should assign Root Property");
        }

        [TestMethod]
        public void ctor_with_viewContext_must_assign_CallingViewContext()
        {
            // Arrange		
            var expectedViewContext = new Mock<ViewContext>().Object;
            var sut = new NavTreeView(new NavRootView("someId", "someName", new Mock<INavNodeProperties>().Object), expectedViewContext);
            // Act
            var actual = sut.CallingViewContext;

            // Assert		
            actual.Should().Be(expectedViewContext);
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


        #endregion


        private static NavTreeView MakeSut(NavRootView root = null, ViewContext callingViewContext = null)
        {
            root = root ?? new NavRootView("someId", "someName", new Mock<INavNodeProperties>().Object);
            callingViewContext = callingViewContext ?? new Mock<ViewContext>().Object;
            return new NavTreeView(root, callingViewContext);
        }


    }
}