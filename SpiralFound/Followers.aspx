<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.master" AutoEventWireup="true" CodeBehind="Followers.aspx.cs" Inherits="SpiralFound.Followers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="centercolumn" runat="server">

<!--HIDDEN LABELS-->
<asp:Literal ID="lit_userName" runat="server" Visible="false"></asp:Literal>

    <div id="usertitle">
		FOLLOWERS
	</div>

        <asp:Repeater ID="RepFollowers" runat="server" DataSourceID="Sql_RepFollow">
            <ItemTemplate>
            	<div id="ownercard">
                    <a href='/<%#Eval("userName")%>' title="">
		                <img src='/Images/Users/<%#Eval("userName")%>.jpg' alt="" />	
		                <div id="owner-title"><%#Eval("userName")%></div>
                    </a>
	            </div>
            </ItemTemplate>
        </asp:Repeater>


<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="Sql_RepFollow" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT B.userName, B.userId FROM tbl_user_follow A LEFT JOIN tbl_user B ON B.userId = A.followerId WHERE A.userId = @userId">
        <SelectParameters>
            <asp:Parameter Name="userId" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->


</asp:Content>
