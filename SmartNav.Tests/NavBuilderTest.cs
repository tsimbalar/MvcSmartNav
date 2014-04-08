﻿using System;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace SmartNav.Tests
{
    [TestClass]
    public class NavBuilderTest
    {
        private readonly Fixture _fixture = new Fixture();

        #region Build

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
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
        [ExpectedException(typeof(ArgumentNullException))]
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
        public void Build_must_create_INavTreeViewModel()
        {
            // Arrange		
            var sut = MakeSut();

            // Act
            var actual = sut.Build(AnyViewContext(), AnyNavSpecification());

            // Assert	
            actual.Should().NotBeNull("should return a non null treeview");
            actual.Should().BeAssignableTo<INavTreeViewModel>();
        }

        [TestMethod]
        public void Build_must_create_tree_with_INavRootViewModel()
        {
            // Arrange		
            var sut = MakeSut();

            // Act
            var actualRoot = sut.Build(AnyViewContext(), AnyNavSpecification()).Root;

            // Assert		
            actualRoot.Should().NotBeNull();
            actualRoot.Should().BeAssignableTo<INavRootViewModel>();
        }

        [TestMethod]
        public void Build_must_create_tree_with_Root_Name_equivalent_to_Spec_Root()
        {
            // Arrange		
            var sut = MakeSut();
            var expectedNodeName = _fixture.Create<string>();
            var specRoot = MockNavNode(mockName:false);
            specRoot.Setup(r => r.Name).Returns(expectedNodeName);
            var spec = NavSpecification(specRoot.Object);

            // Act
            var actualRoot = sut.Build(AnyViewContext(), spec).Root;

            // Assert		
            actualRoot.Name.Should().Be(expectedNodeName);
        }

        [TestMethod]
        public void Build_must_create_tree_with_Root_Visibility_equivalent_to_Spec_Root_EvaluateVisibility()
        {
            // Arrange		
            var sut = MakeSut();
            var viewContext = AnyViewContext();
            var nodeVisibility = _fixture.Create<NodeVisibility>();
            var specRoot = MockNavNode(mockVisibility: false);
            specRoot.Setup(r => r.EvaluateVisibility(viewContext)).Returns(nodeVisibility);
            var spec = NavSpecification(specRoot.Object);

            // Act
            var actualRoot = sut.Build(viewContext, spec).Root;

            // Assert		
            actualRoot.IsVisible.Should().Be(nodeVisibility.IsVisible);
            actualRoot.VisibilityReason.Should().Be(nodeVisibility.Explanation);
        }

        [TestMethod]
        public void Build_must_create_tree_with_ViewContext()
        {
            // Arrange	
            var sut = MakeSut();
            var expectedViewContext = AnyViewContext();

            // Act
            var actual = sut.Build(expectedViewContext, AnyNavSpecification());

            // Assert		
            actual.CallingViewContext.Should().Be(expectedViewContext);
        }


        #endregion

        #region Test Helper Methods

        private static NavBuilder MakeSut()
        {
            return new NavBuilder();
        }

        private INavSpecification AnyNavSpecification()
        {
            return NavSpecification(MockNavNode().Object);
        }

        private INavSpecification NavSpecification(INavNode root)
        {
            var result = new Mock<INavSpecification>(MockBehavior.Strict);
            result.Setup(r => r.Root).Returns(root);

            return result.Object;
        }

        private static ViewContext AnyViewContext()
        {
            return new ViewContext();
        }

        private Mock<INavNode> MockNavNode(bool mockName = true, bool mockVisibility = true)
        {
            var node = new Mock<INavNode>(MockBehavior.Strict);

            if (mockName)
            {
                var name = _fixture.Create<string>();
                node.Setup(n => n.Name).Returns(name);
            }

            if (mockVisibility)
            {
                var visibilityToReturn = _fixture.Create<NodeVisibility>();
                node.Setup(n => n.EvaluateVisibility(It.IsAny<ViewContext>())).Returns(visibilityToReturn);
            }
            
            return node;
        }

        #endregion

    }

    public sealed class NodeVisibility
    {
        private readonly bool _visible;
        private readonly string _explanation;

        public NodeVisibility(bool visible, string explanation)
        {
            _visible = visible;
            _explanation = explanation;
        }

        public bool IsVisible { get { return _visible; } }
        public string Explanation { get { return _explanation; } }
    }

    public interface INavNode
    {
        string Name { get; }
        NodeVisibility EvaluateVisibility(ViewContext viewContext);
    }

    public interface INavSpecification
    {
        INavNode Root { get; }
    }

    internal class NavBuilder
    {
        public INavTreeViewModel Build(ViewContext viewContext, INavSpecification navSpec)
        {
            if (viewContext == null) throw new ArgumentNullException("viewContext");
            if (navSpec == null) throw new ArgumentNullException("navSpec");

            var rootNode = navSpec.Root;
            var rootVisibility = rootNode.EvaluateVisibility(viewContext);
            var rootView = new NavRootView()
                       {
                           Name = rootNode.Name,
                           IsVisible = rootVisibility.IsVisible,
                           VisibilityReason = rootVisibility.Explanation
                       };



            return new NavTreeView(rootView, viewContext);
        }
    }

    public class NavRootView : INavRootViewModel
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public string VisibilityReason { get; set; }
    }

    internal class NavTreeView : INavTreeViewModel
    {
        private readonly NavRootView _navRootView;
        private readonly ViewContext _callingViewContext;

        public NavTreeView(NavRootView navRootView, ViewContext callingViewContext)
        {
            if (navRootView == null) throw new ArgumentNullException("navRootView");
            if (callingViewContext == null) throw new ArgumentNullException("callingViewContext");
            _navRootView = navRootView;
            _callingViewContext = callingViewContext;
        }

        public INavRootViewModel Root { get { return _navRootView; } }
        public ViewContext CallingViewContext { get { return _callingViewContext; } }
    }

    public interface INavTreeViewModel
    {
        INavRootViewModel Root { get; }
        ViewContext CallingViewContext { get; }
    }

    public interface INavRootViewModel
    {
        string Name { get; }
        bool IsVisible { get; }
        string VisibilityReason { get; }
    }
}