using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SmartNav.Tests
{
    [TestClass]
    public class NavBuilderTest
    {

        #region Build

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Build_with_null_NavSpec_must_throw_ArgumentNullException()
        {
            // Arrange		
            var sut = MakeSut();
            
            // Act
            sut.Build(AnyViewContext(), null);

            // Assert		
            // expected exception of type ArgumentNullException
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Build_with_null_ViewContext_must_throw_ArgumentNullException()
        {
            // Arrange		
            var sut = MakeSut();

            // Act
            sut.Build(null, AnyNavSpecification());
            // Assert		
            // expected exception of type ArgumentNullException
        }

        [TestMethod]
        public void Build_with_NavSpec_must_create_NavTree()
        {
            // Arrange		
            var viewContext = AnyViewContext();
            var navSpec = AnyNavSpecification();
            var sut = MakeSut();

            // Act
            var actual = sut.Build(viewContext, navSpec);

            // Assert	
            actual.Should().NotBeNull("should return a non null treeview");
            actual.Should().BeAssignableTo<INavTreeViewModel>();
        }

        [TestMethod]
        public void Build_with_NavSpec_must_build_tree_with_Root()
        {
            // Arrange		
            var sut = MakeSut();
            var viewContext = AnyViewContext();
            var spec = AnyNavSpecification();

            // Act
            var actual = sut.Build(viewContext, spec);

            // Assert		
            actual.Root.Should().NotBeNull();
            actual.Root.Should().BeAssignableTo<INavRootViewModel>();
        }


        #endregion

        #region Test Helper Methods

        private static NavBuilder MakeSut() //TODO : complete list of parameters
        {
            return new NavBuilder();
        }
        
        private static INavSpecification AnyNavSpecification()
        {
            return new Mock<INavSpecification>(MockBehavior.Strict).Object;
        }

        private static ViewContext AnyViewContext()
        {
            return new ViewContext();
        }

        #endregion

    }

    public interface INavNode
    {
    }

    public interface INavSpecification
    {
    }

    internal class NavBuilder
    {
        public INavTreeViewModel Build(ViewContext viewContext, INavSpecification navSpec)
        {
            if (viewContext == null) throw new ArgumentNullException("viewContext");
            if (navSpec == null) throw new ArgumentNullException("navSpec");
            return new NavTreeView(new NavRootView());
        }
    }

    public class NavRootView : INavRootViewModel
    {
    }

    internal class NavTreeView : INavTreeViewModel
    {
        private readonly NavRootView _navRootView;

        public NavTreeView(NavRootView navRootView)
        {
            if (navRootView == null) throw new ArgumentNullException("navRootView");
            _navRootView = navRootView;
        }

        public INavRootViewModel Root { get { return _navRootView; } }
    }

    public interface INavTreeViewModel
    {
        INavRootViewModel Root { get;  }
    }

    public interface INavRootViewModel
    {
    }
}