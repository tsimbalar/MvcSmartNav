using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;

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

    public interface INavNodeProperties
    {
        NodeActivation Activation { get; }
        NodeVisibility Visibility { get; }
        NodeEnablement Enablement { get; }

        string TargetUrl { get; }
    }

    class NavNodeProperties : INavNodeProperties
    {
        public NodeActivation Activation { get; set; }
        public NodeVisibility Visibility { get; set; }
        public NodeEnablement Enablement { get; set; }
        public string TargetUrl { get; set; }
    }

    public sealed class NodeActivation
    {
        private readonly bool _active;
        private readonly string _explanation;

        public NodeActivation(bool active, string explanation)
        {
            _active = active;
            _explanation = explanation;
        }

        public bool IsActive { get { return _active; } }
        public string Explanation { get { return _explanation; } }
    }

    public sealed class NodeEnablement
    {
        private readonly bool _enabled;
        private readonly string _explanation;

        public NodeEnablement(bool enabled, string explanation)
        {
            _enabled = enabled;
            _explanation = explanation;
        }

        public bool IsEnabled { get { return _enabled; } }
        public string Explanation { get { return _explanation; } }
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
        IEnumerable<INavNode> Children { get; }
        NodeVisibility EvaluateVisibility(ViewContext viewContext);
        NodeEnablement EvaluateEnablement(ViewContext viewContext);
        NodeActivation EvaluateActivation(ViewContext viewContext);
        string EvaluateTargetUrl(ViewContext viewContext);
        INavNodeProperties EvaluateNode(ViewContext viewContext);
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

    public class NavRootView : NavItemViewBase
    {
        public NavRootView(string name, INavNodeProperties props)
            : base(name, props)
        {
        }

        public NavRootView AddChild(NavItemView navItemView)
        {
            base.AddChild(navItemView);
            return this;
        }
    }

    public class NavItemView : NavItemViewBase
    {
        public NavItemView(string name, INavNodeProperties props)
            : base(name, props)
        {
        }

        public NavItemView AddChild(NavItemView navItemView)
        {
            base.AddChild(navItemView);
            return this;
        }
    }

    public abstract class NavItemViewBase : INavComponentViewModel
    {
        private readonly string _name;
        private readonly INavNodeProperties _props;
        private readonly List<INavComponentViewModel> _children;

        protected NavItemViewBase(string name, INavNodeProperties props)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (props == null) throw new ArgumentNullException("props");
            _name = name;
            _props = props;
            _children = new List<INavComponentViewModel>();
        }

        public string Name { get { return _name; } }

        public bool IsVisible
        {
            get { return _props.Visibility.IsVisible; }
        }

        public string VisibilityReason
        {
            get { return _props.Visibility.Explanation; }
        }

        public bool IsEnabled
        {
            get { return _props.Enablement.IsEnabled; }
        }

        public string EnablementReason
        {
            get { return _props.Enablement.Explanation; }
        }

        public bool IsActive
        {
            get { return _props.Activation.IsActive; }
        }

        public string ActivationReason
        {
            get { return _props.Activation.Explanation; }
        }

        public string Url
        {
            get { return _props.TargetUrl; }
        }

        public IEnumerable<INavComponentViewModel> Children { get { return _children; } }

        protected void AddChild(INavComponentViewModel child)
        {
            _children.Add(child);
        }

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

        public INavComponentViewModel Root { get { return _navRootView; } }
        public ViewContext CallingViewContext { get { return _callingViewContext; } }
    }

    public interface INavTreeViewModel
    {
        INavComponentViewModel Root { get; }
        ViewContext CallingViewContext { get; }
    }

    public interface INavComponentViewModel
    {
        string Name { get; }
        bool IsVisible { get; }
        string VisibilityReason { get; }
        bool IsEnabled { get; }
        string EnablementReason { get; }
        bool IsActive { get; }
        string ActivationReason { get; }
        string Url { get; }
        IEnumerable<INavComponentViewModel> Children { get; }
    }
}