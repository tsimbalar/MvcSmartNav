using System;
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
        public void Build_must_create_tree_with_INavRootViewModel()
        {
            // Arrange		
            var sut = MakeSut();

            // Act
            var actualRoot = sut.Build(any.ViewContext(), any.NavSpecification()).Root;

            // Assert		
            actualRoot.Should().NotBeNull();
            actualRoot.Should().BeAssignableTo<INavRootViewModel>();
        }

        #endregion

        #region Root of NavTree


        [TestMethod]
        public void Build_must_create_tree_with_Root_Name_equivalent_to_Spec_Root()
        {
            // Arrange		
            var sut = MakeSut();
            var expectedNodeName = _fixture.Create<string>();
            var specRoot = any.MockNavNode(mockName: false);
            specRoot.Setup(r => r.Name).Returns(expectedNodeName);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualRoot = sut.Build(any.ViewContext(), spec).Root;

            // Assert		
            actualRoot.Name.Should().Be(expectedNodeName);
        }

        [TestMethod]
        public void Build_must_create_tree_with_Root_Visibility_equivalent_to_Spec_Root_EvaluateVisibility()
        {
            // Arrange		
            var sut = MakeSut();
            var viewContext = any.ViewContext();
            var nodeVisibility = _fixture.Create<NodeVisibility>();
            var specRoot = any.MockNavNode(mockVisibility: false);
            specRoot.Setup(r => r.EvaluateVisibility(viewContext)).Returns(nodeVisibility);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualRoot = sut.Build(viewContext, spec).Root;

            // Assert		
            actualRoot.IsVisible.Should().Be(nodeVisibility.IsVisible, "Root.IsVisible");
            actualRoot.VisibilityReason.Should().Be(nodeVisibility.Explanation, "Root.VisibilityReason");
        }

        [TestMethod]
        public void Build_must_create_tree_with_Root_Enablement_equivalent_to_Spec_Root_EvaluateEnablement()
        {
            // Arrange		
            var sut = MakeSut();
            var viewContext = any.ViewContext();
            var nodeEnablement = _fixture.Create<NodeEnablement>();
            var specRoot = any.MockNavNode(mockEnablement: false);
            specRoot.Setup(r => r.EvaluateEnablement(viewContext)).Returns(nodeEnablement);
            var spec = any.NavSpecification(specRoot.Object);

            // Act
            var actualRoot = sut.Build(viewContext, spec).Root;

            // Assert		
            actualRoot.IsEnabled.Should().Be(nodeEnablement.IsEnabled, "Root.IsEnabled");
            actualRoot.EnablementReason.Should().Be(nodeEnablement.Explanation, "Root.EnablementReason");
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


            public Mock<INavNode> MockNavNode(bool mockName = true, bool mockVisibility = true, bool mockEnablement = true)
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

                if (mockEnablement)
                {
                    var enablementToReturn = _fixture.Create<NodeEnablement>();
                    node.Setup(n => n.EvaluateEnablement(It.IsAny<ViewContext>())).Returns(enablementToReturn);
                }

                return node;
            }

            public ViewContext ViewContext()
            {
                return new ViewContext();
            }
        }

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
        NodeVisibility EvaluateVisibility(ViewContext viewContext);
        NodeEnablement EvaluateEnablement(ViewContext viewContext);
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
            var rootEnablement = rootNode.EvaluateEnablement(viewContext);
            var rootView = new NavRootView()
                       {
                           Name = rootNode.Name,

                           IsVisible = rootVisibility.IsVisible,
                           VisibilityReason = rootVisibility.Explanation,

                           IsEnabled = rootEnablement.IsEnabled,
                           EnablementReason = rootEnablement.Explanation
                       };



            return new NavTreeView(rootView, viewContext);
        }
    }

    public class NavRootView : INavRootViewModel
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public string VisibilityReason { get; set; }
        public bool IsEnabled { get; set; }
        public string EnablementReason { get; set; }
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
        bool IsEnabled { get; }
        string EnablementReason { get; }
    }
}