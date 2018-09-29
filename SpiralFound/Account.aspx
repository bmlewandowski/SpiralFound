<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="SpiralFound.Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <!--LOAD STYLESHEETS/JS -->
    <link rel="stylesheet" type="text/css" href="/Styles/page.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="col_left">
	&nbsp; 
</div>
 
<div id="col_center">
	<div id="page">
 
		<div id="page-top"></div> 
		<div id="page-content">
 
            <img id="logo" src="/images/interface/commands/signup.png" style="margin-left:80px;" alt="Sign Up" />
			<div id="page-descrip">Sign Up to SpiralFound is easy, just fill out the following and we will create you an account and you can get started creating cool books and pages of your favorite stuff!</div>
       

                <p style="font-size:30px;">Welcome <asp:Literal ID="lit_fbUserName" runat="server"></asp:Literal>! Now Let's Sign Up.</p>

            <div style="float:left; margin-left:100px;">
                <asp:Image ID="img_fbUserImage" runat="server" />
            </div>        
                
            <div style="text-align:right; margin-right:100px;">
                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Choose Username: </asp:Label>
                                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                ToolTip="User Name is required." ValidationGroup="CreateUserWizard" ForeColor="Red">X</asp:RequiredFieldValidator>
                                                <br />
                                                <asp:RegularExpressionValidator ID="UserNameValid" ControlToValidate="UserName" runat="server" ErrorMessage="At least 5 Numbers and Letters, No Spaces"
                                                 ValidationGroup="CreateUserWizard" ForeColor="Red" ValidationExpression="[0-9a-zA-Z]{5,}"></asp:RegularExpressionValidator>

                                                 <br />
                                                 <br />

                                           <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">Your email: </asp:Label>
                                           <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                                ControlToValidate="Email" ErrorMessage="E-mail is required." 
                                                ToolTip="E-mail is required." ValidationGroup="CreateUserWizard" ForeColor="Red">X</asp:RequiredFieldValidator>
                                                <br />
                                            <asp:RegularExpressionValidator ID="EmailValid" ControlToValidate="Email" runat="server" ErrorMessage="Invalid Email Address" 
                                            ValidationGroup="CreateUserWizard" ForeColor="Red" ValidationExpression="[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b"></asp:RegularExpressionValidator>
                                  
                                                 <br />
                                                 <br />
                                                                                             
                                           <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">New Password: </asp:Label>
                                           <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                ControlToValidate="Password" ErrorMessage="Password is required." 
                                                ToolTip="Password is required." ValidationGroup="CreateUserWizard" ForeColor="Red">X</asp:RequiredFieldValidator>
                                                <br />
                                            <asp:RegularExpressionValidator ID="PasswordValid" ControlToValidate="Password" runat="server" ErrorMessage="At least 7 Characters, No Spaces"
                                                 ValidationGroup="CreateUserWizard" ForeColor="Red" ValidationExpression="^\S{6}\S+$"></asp:RegularExpressionValidator>

                                                 <br />
                                                 <br />

                                           <asp:Label ID="ConfirmPasswordLabel" runat="server" 
                                                AssociatedControlID="ConfirmPassword">Confirm Password: </asp:Label>
                                           <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
                                                ControlToValidate="ConfirmPassword" 
                                                ErrorMessage="Confirm Password is required." 
                                                ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard" ForeColor="Red">X</asp:RequiredFieldValidator>
                                                <br />

                             
                                            <asp:CompareValidator ID="PasswordCompare" runat="server" 
                                                ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                                Display="Dynamic" ForeColor="Red"
                                                ErrorMessage="Bummer! The Passwords don't match." 
                                                ValidationGroup="CreateUserWizard"></asp:CompareValidator>
                                                <br />
                                      <asp:Button ID="btn_Register" runat="server" Text="Get Started!" onclick="btn_Register_Click" ValidationGroup="CreateUserWizard" />
                                <br />
                                            <span style="color:Red;"><asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal></span>

</div>
</div>
		<div id="page-bottom"></div>
	</div>
</div>

<div id="col_right">
	&nbsp;
</div> 
     
</asp:Content>
