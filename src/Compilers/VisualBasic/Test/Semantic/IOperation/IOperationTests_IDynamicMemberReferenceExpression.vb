﻿Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.UnitTests.Semantics

    Partial Public Class IOperationTests
        Inherits SemanticModelTestBase
        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_SimplePropertyAccess()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Foo'BIND:"d.Foo"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.Foo')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: Foo, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Foo')
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_GenericPropertyAccess()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Foo(Of String)'BIND:"d.Foo(Of String)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.Foo(Of String)')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: Foo, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Foo(Of String)')
        Type Arguments: System.String
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_InvalidGenericPropertyAccess()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Foo(Of)'BIND:"d.Foo(Of)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'd.Foo(Of)')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: Foo, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object, IsInvalid) (Syntax: 'd.Foo(Of)')
        Type Arguments: ?
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30182: Type expected.
        d.Foo(Of)'BIND:"d.Foo(Of)"
                ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_SimpleMethodCall()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Foo()'BIND:"d.Foo()"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.Foo()')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: Foo, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Foo')
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_InvalidMethodCall_MissingParen()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Foo('BIND:"d.Foo("
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'd.Foo(')
  Children(2):
      IDynamicMemberReferenceExpression (Member name: Foo, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Foo')
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
      IConversionExpression (ConversionKind.Invalid, Implicit) (OperationKind.ConversionExpression, Type: System.Object, IsInvalid) (Syntax: '')
        IInvalidExpression (OperationKind.InvalidExpression, Type: ?, IsInvalid) (Syntax: '')
          Children(0)
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30198: ')' expected.
        d.Foo('BIND:"d.Foo("
              ~
BC30201: Expression expected.
        d.Foo('BIND:"d.Foo("
              ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_GenericMethodCall_SingleGeneric()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.GetValue(Of String)()'BIND:"d.GetValue(Of String)()"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.GetValue(Of String)()')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: GetValue, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.GetValue(Of String)')
        Type Arguments: System.String
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_GenericMethodCall_MultipleGeneric()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.GetValue(Of String, Integer)()'BIND:"d.GetValue(Of String, Integer)()"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.GetValue( ...  Integer)()')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: GetValue, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.GetValue( ... g, Integer)')
        Type Arguments:
          System.String
          System.Int32
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_GenericMethodCall_InvalidGenericParameter()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.GetValue(Of String,)()'BIND:"d.GetValue(Of String,)()"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'd.GetValue(Of String,)()')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: GetValue, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object, IsInvalid) (Syntax: 'd.GetValue(Of String,)')
        Type Arguments:
          System.String
          ?
        Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30182: Type expected.
        d.GetValue(Of String,)()'BIND:"d.GetValue(Of String,)()"
                             ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_NestedPropertyAccess()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Prop1.Prop2'BIND:"d.Prop1.Prop2"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.Prop1.Prop2')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: Prop2, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Prop1.Prop2')
        Instance Receiver: IDynamicMemberReferenceExpression (Member name: Prop1, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Prop1')
            Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_NestedMethodAccess()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Method1().Method2()'BIND:"d.Method1().Method2()"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.Method1().Method2()')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: Method2, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Method1().Method2')
        Instance Receiver: IOperation:  (OperationKind.None) (Syntax: 'd.Method1()')
            Children(1):
                IDynamicMemberReferenceExpression (Member name: Method1, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Method1')
                  Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_NestedPropertyAndMethodAccess()
            Dim source = <![CDATA[
Option Strict Off
Module Program
    Sub Main(args As String())
        Dim d = Nothing
        d.Prop1.Method2()'BIND:"d.Prop1.Method2()"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'd.Prop1.Method2()')
  Children(1):
      IDynamicMemberReferenceExpression (Member name: Method2, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Prop1.Method2')
        Instance Receiver: IDynamicMemberReferenceExpression (Member name: Prop1, Containing Type: Object) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'd.Prop1')
            Instance Receiver: ILocalReferenceExpression: d (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'd')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_LateBoundModuleFunction()
            Dim source = <![CDATA[
Option Strict Off
Imports System.Collections.Generic

Module Module1
    Sub Main()
        Dim x As Object = New List(Of Integer)()
        fun(x)'BIND:"fun(x)"
    End Sub

    Sub fun(Of X)(ByVal a As List(Of X))
    End Sub

End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'fun(x)')
  Children(2):
      IDynamicMemberReferenceExpression (Member name: fun, Containing Type: Module1) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'fun')
        Instance Receiver: null
      ILocalReferenceExpression: x (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'x')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <Fact()>
        Public Sub IDynamicMemberReferenceExpression_LateBoundClassFunction()
            Dim source = <![CDATA[
Option Strict Off
Imports System.Collections.Generic

Module Module1
    Sub Main()
        Dim x As Object = New List(Of Integer)()
        Dim c1 As New C1
        c1.fun(x)'BIND:"c1.fun(x)"
    End Sub

    Class C1
        Sub fun(Of X)(ByVal a As List(Of X))
        End Sub
    End Class
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IOperation:  (OperationKind.None) (Syntax: 'c1.fun(x)')
  Children(2):
      IDynamicMemberReferenceExpression (Member name: fun, Containing Type: Module1.C1) (OperationKind.DynamicAccessExpression, Type: System.Object) (Syntax: 'c1.fun')
        Instance Receiver: ILocalReferenceExpression: c1 (OperationKind.LocalReferenceExpression, Type: Module1.C1) (Syntax: 'c1')
      ILocalReferenceExpression: x (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'x')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of InvocationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

    End Class
End Namespace

