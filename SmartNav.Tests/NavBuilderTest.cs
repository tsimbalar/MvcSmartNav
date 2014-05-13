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
            specRoot.Setup(r => r.Evaluate(viewContext)).Returns(navNodeProperties);
            var spec = any.NavSpecification(specRoot.Object);

            // Acts
            var actualRoot = sut.Build(viewContext, spec).Root;

            // Assert	
            AssertViewMatchesSpecDynamicProperties(actualRoot, navNodeProperties);
        }

        [TestMethod]
        public void Build_must_create_Root_with_Level_0()
        {
            // Arrange		
            var sut = MakeSut();
            var specRoot = any.MockNavNode().Object;
            var spec = any.NavSpecification(specRoot);

            // Act
            var actualRoot = sut.Build(any.ViewContext(), spec).Root;

            // Assert		
            actualRoot.Level.Should().Be(0);
        }

        [TestMethod]
        public void Build_must_create_tree_with_Root_static_props_equivalent_to_Spec_Root()
        {
            // Arrange		
            var sut = MakeSut();
            var expectedNodeName = _fixture.Create<string>();
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Id).Returns(expectedNodeName);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualRoot = sut.Build(any.ViewContext(), spec).Root;

            // Assert	
            AssertViewMatchesSpecStaticProperties(actualRoot, specRoot.Object);
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
        public void Build_with_Spec_with_child_must_create_child_with_static_props_from_spec()
        {
            // Arrange		
            var sut = MakeSut();
            var childName = _fixture.Create<string>();
            var childSpec = any.MockNavNode();
            childSpec.Setup(s => s.Id).Returns(childName);
            var children = new List<INavNode> { childSpec.Object };
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(children);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualChild = sut.Build(any.ViewContext(), spec).Root.Children.SingleOrDefault();

            // Assert		
            actualChild.Should().NotBeNull("it should return child");
            AssertViewMatchesSpecStaticProperties(actualChild, childSpec.Object);
        }

        [TestMethod]
        public void Build_with_Spec_with_child_must_create_child_with_level_1()
        {
            // Arrange		
            var sut = MakeSut();
            var childSpec = any.MockNavNode().Object;
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(new List<INavNode> { childSpec });
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualChild = sut.Build(any.ViewContext(), spec).Root.Children.Single();

            // Assert		
            actualChild.Level.Should().Be(1, "level should be 1");
        }

        [TestMethod]
        public void Build_with_spec_with_child_must_create_child_with_dynamic_Properties_equivalent_to_spec()
        {
            // Arrange		
            var sut = MakeSut();
            var viewContext = any.ViewContext();

            var childSpec = any.MockNavNode();
            var navNodeProperties = _fixture.Create<NavNodeProperties>();
            childSpec.Setup(r => r.Evaluate(viewContext)).Returns(navNodeProperties);
            var children = new List<INavNode> { childSpec.Object };
            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(children);

            var spec = any.NavSpecification(specRoot.Object);

            // Acts
            var actualChild = sut.Build(viewContext, spec).Root.Children.SingleOrDefault();

            // Assert	
            AssertViewMatchesSpecDynamicProperties(actualChild, navNodeProperties);
        }

        [TestMethod]
        public void Build_with_spec_with_2_children_must_create_2_children()
        {
            // Arrange		
            var sut = MakeSut();

            var childSpecs = new List<INavNode> { any.MockNavNode().Object, any.MockNavNode().Object, any.MockNavNode().Object };

            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(childSpecs);

            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualChildren = sut.Build(any.ViewContext(), spec).Root.Children.ToList();

            // Assert		
            actualChildren.Should().HaveCount(childSpecs.Count);
        }

        [TestMethod]
        public void Build_with_spec_children_level_2_must_create_2_levels()
        {
            // Arrange		
            var sut = MakeSut();
            var grandChildSpec = any.MockNavNode().Object;
            var childSpec = any.MockNavNode();
            childSpec.Setup(s => s.Children).Returns(new List<INavNode> { grandChildSpec });

            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(new List<INavNode> { childSpec.Object });

            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualGrandChild = sut.Build(any.ViewContext(), spec).Root.Children.Single().Children.SingleOrDefault();
            // Assert		
            actualGrandChild.Should().NotBeNull("there should be 2 levels");
            AssertViewMatchesSpecStaticProperties(actualGrandChild, grandChildSpec);
        }

        [TestMethod]
        public void Build_with_Spec_with_grandChild_must_create_grandChild_with_level_2()
        {
            // Arrange		
            var sut = MakeSut();
            var grandChildSpec = any.MockNavNode().Object;
            var childSpec = any.MockNavNode();
            childSpec.Setup(s => s.Children).Returns(new List<INavNode> { grandChildSpec });

            var specRoot = any.MockNavNode();
            specRoot.Setup(r => r.Children).Returns(new List<INavNode> { childSpec.Object });
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualGrandChild = sut.Build(any.ViewContext(), spec).Root.Children.Single().Children.Single();

            // Assert		
            actualGrandChild.Level.Should().Be(2, "should have level 2");

        }

        #endregion

        #region Test Helper Methods

        private static NavBuilder MakeSut()
        {
            return new NavBuilder();
        }

        private static void AssertViewMatchesSpecStaticProperties(INavComponentViewModel actualViewModel, INavNode nodeSpec)
        {
            actualViewModel.Name.Should().Be(nodeSpec.Name, "Name should match");
            actualViewModel.Id.Should().Be(nodeSpec.Id, "Id should match");
        }

        private static void AssertViewMatchesSpecDynamicProperties(INavComponentViewModel actualRoot, INavNodeProperties navNodeProperties)
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


            public INavTreeSpecification NavSpecification()
            {
                return NavSpecification(MockNavNode().Object);
            }

            public INavTreeSpecification NavSpecification(INavNode root)
            {
                var result = new Mock<INavTreeSpecification>(MockBehavior.Strict);
                result.Setup(r => r.Root).Returns(root);

                return result.Object;
            }


            public Mock<INavNode> MockNavNode()
            {
                var node = new Mock<INavNode>(MockBehavior.Strict);
                node.Setup(n => n.Children).Returns(Enumerable.Empty<INavNode>);

                var id = _fixture.Create<string>("id");
                node.Setup(n => n.Id).Returns(id);

                var name = _fixture.Create("name");
                node.Setup(n => n.Name).Returns(name);


                var props = _fixture.Create<NavNodeProperties>();
                node.Setup(n => n.Evaluate(It.IsAny<ViewContext>())).Returns(props);

                return node;
            }

            public ViewContext ViewContext()
            {
                return new ViewContext();
            }
        }
    }

    internal class NavBuilder
    {
        public INavTreeViewModel Build(ViewContext viewContext, INavTreeSpecification navTreeSpec)
        {
            if (viewContext == null) throw new ArgumentNullException("viewContext");
            if (navTreeSpec == null) throw new ArgumentNullException("navTreeSpec");

            var rootNode = navTreeSpec.Root;
            var rootProperties = rootNode.Evaluate(viewContext);
            var rootView = new NavRootView(rootNode.Id, rootNode.Name, rootProperties);

            AddChildrenRecursively(rootView, rootNode.Children, viewContext);

            return new NavTreeView(rootView, viewContext);
        }

        private void AddChildrenRecursively(NavItemViewBase rootToAddTo, IEnumerable<INavNode> childrenSpecs, ViewContext context)
        {
            foreach (var navNode in childrenSpecs)
            {
                var nodeProperties = navNode.Evaluate(context);
                var child = new NavItemView(navNode.Id, rootToAddTo.Level + 1, navNode.Name, nodeProperties);
                AddChildrenRecursively(child, navNode.Children, context);
                rootToAddTo.AddChild(child);
            }
        }


    }
}