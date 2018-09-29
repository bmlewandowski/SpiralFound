<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="SpiralFound.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="centercolumn" runat="server">

<!--HIDDEN LABELS-->
<asp:Literal ID="lit_userName" runat="server" Visible="false"></asp:Literal>


    <div id="usertitle">
		<asp:Literal ID="lit_authorsCollections" runat="server"></asp:Literal>'S BOOKS
	</div>
    

 <!--BEGIN ADD BOX -->
		<div class="commands-addbook-box" id="commands-boxadd">
        <div class="command-boxtop">
            <img src="/images/interface/commands/command-close.png" onclick="hide_command_box('commands-boxadd')" class="command-boxclose" alt="CLOSE" />
        </div>
        <div class="command-boxcontent">

           <strong>ADD NEW BOOK</strong>
            <br />
            <br />
            <strong>Title:</strong> <asp:TextBox ID="txt_add" runat="server" Width="350px" MaxLength="420"></asp:TextBox>
            <asp:RequiredFieldValidator ID="val_add" runat="server" ControlToValidate="txt_add" ErrorMessage="Required" ValidationGroup="Add" ForeColor="Red">X</asp:RequiredFieldValidator>
            <br />
            <br />
            <strong>Description:</strong>
            <br />
            <asp:TextBox ID="txt_addDescription" runat="server" Height="75px" Width="400px" TextMode="MultiLine" />
            <br />
            <br />
            <asp:Button ID="btn_add" runat="server" Text="Add Book" ValidationGroup="Add" onclick="btn_add_Click" Height="50px" />

		</div>
        <div class="command-boxbottom">
        </div>
        </div>
<!--END ADD BOX -->
                   

            <div ID="btn_addBook" runat="server" class="addbookthumb" visible="false">
			<img src="/images/interface/btn_add_book.png" alt="" onclick="show_addbook_box('commands-boxadd')" />
            </div>
		

        <asp:Repeater ID="RepBooks" runat="server" DataSourceID="Sql_RepBooks">
          
            <ItemTemplate>
 
		<a href='/Book/<%#Eval("collectionId")%>' title='<%# SpiralFound.Helper.Trim(Eval("collectionName"), 100)%>'><div class="bookthumb">
			<div class="bookthumb-titletext"><%# SpiralFound.Helper.Trim(Eval("collectionName"), 15)%></div>
			<div class="bookthumb-cover"><img src='/images/books/<%#Eval("collectionImage")%>_s.jpg' alt="" /></div>
			<div class="bookthumb-descrip"><%# SpiralFound.Helper.Trim(Eval("collectionDescription"), 120)%></div>
		</div></a>

            </ItemTemplate>
            
        </asp:Repeater>
	

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="Sql_RepBooks" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT * FROM tbl_collection WHERE userId = @userId">
        <SelectParameters>
            <asp:Parameter Name="userId" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->


</asp:Content>
