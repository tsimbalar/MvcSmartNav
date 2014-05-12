using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;
using SmartNav.Tests.NavSpec;
using SmartNav.Tests.NavView;

namespace SmartNav.Tests
{
    [TestClass]
    public class NavBuilderTest
    {
        private readonly Fixture _fixture = new Fixture();

        #region Build validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Build_with_null_NavSpec_must_throw_ArgumentNullException()
        {
            // Arrange		
            var sut = MakeSut();

            // Act
            sut.Build(any.ViewContext(), null);

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
            sut.Build(null, any.NavSpecification());
            // Assert		
            // expected exception of type ArgumentNullException
        }
        #endregion
        #region NavTree

        [TestMethod]
        public void Build_must_create_INavTreeViewModel()
        {
            // Arrange		
            var sut = MakeSut();

            // Act
            var actual = sut.Build(any.ViewContext(), any.NavSpecification());

            // Assert	
            actual.Should().NotBeNull("should return a non null treeview");
            actual.Should().BeAssignableTo<INavTreeViewModel>();
        }


        [TestMethod]
        public void Build_must_create_tree_with_ViewContext()
        {
            // Arrange	
            var sut = MakeSut();
            var expectedViewContext = any.ViewContext();

            // Act
            var actual = sut.Build(expectedViewContext, any.NavSpecification());

            // Assert		
            actual.CallingViewContext.Should().Be(expectedViewContext);
        }


        [TestMethod]
        public void Build_must_create_tree_with_INavComponentViewModel()
        {
            // Arrange		
            var sut = MakeSut();

            // Act
            var actualRoot = sut.Build(any.ViewContext(), any.NavSpecification()).Root;

            // Assert		
            actualRoot.Should().NotBeNull();
            actualRoot.Should().BeAssignableTo<INavComponentViewModel>();
        }

        #endregion

        #region Root of NavTree


        [TestMethod]
        public void Build_must_create_tree_with_Root_Properties_equivalent_to_Spec_EvaluateNode()
        {
            // Arrange		
            var sut = MakeSut();
            var viewContext = any.ViewContext();

            var specRoot = any.MockNavNode();
            var navNodeProperties = _fixture.Create<NavNodeProperties>();
            specRoot.Setup(r => r.EvaluateNode(viewContext)).Returns(navNodeProperties);
            var spec = any.NavSpecification(specRoot.Object);

            // Acts
            var actualRoot = sut.Build(viewContext, spec).Root;

            // Assert	
            AssertNodeViewMatchesProperties(actualRoot, navNodeProperties);
        }

        [TestMethod]
        public void Build_must_create_tree_with_Root_Name_equivalent_to_Spec_Root()
        {
            // Arrange		
            var sut = MakeSut();
            var expectedNodeName = _fixture.Create<string>();
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Name).Returns(expectedNodeName);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualRoot = sut.Build(any.ViewContext(), spec).Root;

            // Assert		
            actualRoot.Name.Should().Be(expectedNodeName);
        }

        #endregion

        #region Tree structure

        [TestMethod]
        public void Build_with_Spec_with_one_child_must_create_child()
        {
            // Arrange		
            var sut = MakeSut();
            var childSpec = any.MockNavNode().Object;
            var children = new List<INavNode> { childSpec };
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(children);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualChildren = sut.Build(any.ViewContext(), spec).Root.Children.ToList();

            // Assert		
            actualChildren.Should().NotBeNull("it should return children");
            actualChildren.Should().HaveCount(1, "there should be one child");
        }

        [TestMethod]
        public void Build_with_Spec_with_child_must_create_child_with_Name_from_spec()
        {
            // Arrange		
            var sut = MakeSut();
            var childName = _fixture.Create<string>();
            var childSpec = any.MockNavNode();
            childSpec.Setup(s => s.Name).Returns(childName);
            var children = new List<INavNode> { childSpec.Object };
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(children);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualChild = sut.Build(any.ViewContext(), spec).Root.Children.SingleOrDefault();

            // Assert		
            actualChild.Should().NotBeNull("it should return child");
            actualChild.Name.Should().Be(childName);
        }

        [TestMethod]
        public void Build_with_spec_with_child_must_create_child_with_Properties_equivalent_to_spec()
        {
            // Arrange		
            var sut = MakeSut();
            var viewContext = any.ViewContext();

            var childSpec = any.MockNavNode();
            var navNodeProperties = _fixture.Create<NavNodeProperties>();
            childSpec.Setup(r => r.EvaluateNode(viewContext)).Returns(navNodeProperties);
            var children = new List<INavNode> { childSpec.Object };
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(children);

            var spec = any.NavSpecification(specRoot.Object);

            // Acts
            var actualChild = sut.Build(viewContext, spec).Root.Children.SingleOrDefault();

            // Assert	
            AssertNodeViewMatchesProperties(actualChild, navNodeProperties);
        }

        #endregion


        #region Test Helper Methods

        private static NavBuilder MakeSut()
        {
            return new NavBuilder();
        }

        #endregion

        // ReSharper disable InconsistentNaming

        private Mother any { get { return new NavBuilderTest.Mother(_fixture); } }

        // ReSharper restore InconsistentNaming

        private class Mother
        {
            private readonly Fixture _fixture;

            public Mother(Fixture fixture)
            {
                _fixture = fixture;
            }


            public INavSpecification NavSpecification()
            {
                return NavSpecification(MockNavNode().Object);
            }

            public INavSpecification NavSpecification(INavNode root)
            {
                var result = new Mock<INavSpecification>(MockBehavior.Strict);
                result.Setup(r => r.Root).Returns(root);

                return result.Object;
            }


            public Mock<INavNode> MockNavNode()
            {
                var node = new Mock<INavNode>(MockBehavior.Strict);
                node.Setup(n => n.Children).Returns(Enumerable.Empty<INavNode>);

                var name = _fixture.Create<string>();
                node.Setup(n => n.Name).Returns(name);


                var props = _fixture.Create<NavNodeProperties>();
                node.Setup(n => n.EvaluateNode(It.IsAny<ViewContext>())).Returns(props);

                return node;
            }

            public ViewContext ViewContext()
            {
                return new ViewContext();
            }
        }

        private static void AssertNodeViewMatchesProperties(INavComponentViewModel actualRoot, NavNodeProperties navNodeProperties)
        {

            actualRoot.AsSource().OfLikeness<INavNodeProperties>()
                .With(p => p.Activation)
                    .EqualsWhen((vm, props) => vm.IsActive == props.Activation.IsActive
                        && vm.ActivationReason == props.Activation.Explanation)
                .With(p => p.Visibility)
                    .EqualsWhen((vm, props) => vm.IsVisible == props.Visibility.IsVisible
                        && vm.VisibilityReason == props.Visibility.Explanation)
                .With(p => p.Enablement)
                    .EqualsWhen((vm, props) => vm.IsEnabled == props.Enablement.IsEnabled
                        && vm.EnablementReason == props.Enablement.Explanation)
                .With(p => p.TargetUrl)
                    .EqualsWhen((vm, props) => vm.Url == props.TargetUrl)
                .ShouldEqual(navNodeProperties);
        }
    }

    internal class NavBuilder
    {
        public INavTreeViewModel Build(ViewContext viewContext, INavSpecification navSpec)
        {
            if (viewContext == null) throw new ArgumentNullException("viewContext");
            if (navSpec == null) throw new ArgumentNullException("navSpec");


            var rootNode = navSpec.Root;
            var rootProperties = rootNode.EvaluateNode(viewContext);
            var rootView = new NavRootView(rootNode.Name, rootProperties);
            foreach (var navNode in rootNode.Children)
            {
                var nodeProperties = navNode.EvaluateNode(viewContext);
                rootView.AddChild(new NavItemView(navNode.Name, nodeProperties));
            }

            return new NavTreeView(rootView, viewContext);
        }
    }
}