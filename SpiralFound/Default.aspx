<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SpiralFound.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <!--LOAD STYLESHEETS/JS -->
	<link rel="stylesheet" type="text/css" href="Styles/default.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- BEGIN BOOKS -->
	<div id="topbooks">

        <asp:Repeater ID="RepBooks" runat="server" DataSourceID="Sql_RepBooks">
            <ItemTemplate>
 
		<a href='/Book/<%#Eval("collectionId")%>' title='<%# SpiralFound.Helper.Trim(Eval("collectionName"), 100)%>'><div class="bookthumb">
			<div class="bookthumb-titletext"><%# SpiralFound.Helper.Trim(Eval("collectionName"), 15)%></div>
			<div class="bookthumb-cover"><img src='/images/books/<%#Eval("collectionImage")%>_s.jpg' alt="" /></div>
			<div class="bookthumb-descrip"><%# SpiralFound.Helper.Trim(Eval("collectionDescription"), 120)%></div>
		</div></a>

            </ItemTemplate>
            
        </asp:Repeater>
    
    <!-- END BOOKS -->

    <!-- BEGIN PAGES -->
	</div><div id="toppages">

        <asp:Repeater ID="RepPages" runat="server" DataSourceID="Sql_RepPages">
            <ItemTemplate>

		<a href='/Page/<%#Eval("groupId")%>' title='<%# SpiralFound.Helper.Trim(Eval("groupName"), 100)%>'>
		<div class="pagethumb">	
			<img src="/images/pages/page-thumbshot.png" alt='<%# SpiralFound.Helper.Trim(Eval("groupName"), 100)%>' />
			<div class="pagethumb-titletext"><%# SpiralFound.Helper.Trim(Eval("groupName"), 15)%></div>
			<div class="pagethumb-descrip"><%# SpiralFound.Helper.Trim(Eval("groupDescription"), 100)%></div>
		</div>
		</a>

            </ItemTemplate>
        </asp:Repeater>
  
    <!-- END PAGES -->

    <!-- BEGIN ITEMS -->
	</div><div id="topitems">

		<div id="container">

        <asp:Repeater ID="RepItems" runat="server" DataSourceID="Sql_RepItems">
            <ItemTemplate>

			<a href='/Item/<%#Eval("itemId")%>' title='<%# SpiralFound.Helper.Trim(Eval("itemName"), 100)%>'>
			<div class="item">
				<div class="itemtop"></div>	
				<div class="itemthumb-content"><img src='/images/items/<%#Eval("itemImage")%>_s.jpg' width="170" alt='<%# SpiralFound.Helper.Trim(Eval("itemName"), 100)%>' /></div>	
				<div class="itemtitle"><p class="itemthumb-titletext"><%# SpiralFound.Helper.Trim(Eval("itemName"), 20)%></p></div>
				<div class="itemthumb-descrip"><p class="itemthumb-descriptext"><%# SpiralFound.Helper.Trim(Eval("itemDescription"), 75)%></p></div>
				<div class="itemthumb-bottom"></div>
			</div>
			</a>

            </ItemTemplate>
        </asp:Repeater>
		</div>

	</div>
    <!-- END ITEMS -->

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="Sql_RepBooks" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT TOP 75 * FROM tbl_collection">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Sql_RepPages" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT TOP 75 * FROM tbl_group">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Sql_RepItems" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT TOP 75 * FROM tbl_item">
    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->

</asp:Content>
