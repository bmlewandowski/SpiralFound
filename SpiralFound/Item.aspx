<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Item.aspx.cs" Inherits="SpiralFound.Item" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <!--LOAD STYLESHEETS/JS -->
	<link rel="stylesheet" type="text/css" href="/Styles/item.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_Items" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<div id="col_left">

    <!--HIDDEN LABELS-->
    <asp:Literal ID="lit_itemUrl" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="lit_itemId" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="lit_groupId" runat="server" Visible="false"></asp:Literal>


	<div id="ownercard">

		<div id="ownercardleft">
			<div id="ownercard-top"><asp:HyperLink ID="hyp_AuthorName" runat="server" ForeColor="#FFFFFF"></asp:HyperLink></div>
			<div id="ownercard-middle"><asp:ImageButton ID="imb_author" runat="server" CssClass="ownercard-userpic" /></div>
			<div id="ownercard-bottom"></div>
		</div>

		<div id="ownercardright">
			<img src="/images/interface/owner/ownercard-stack.png" alt="" />
		</div>

		<div class="clearfloat"></div>

	</div>

	<div id="commands">

<!--BEGIN SAVE BOX -->
		<div class="commands-box" id="commands-boxsave">
        <div class="command-boxtop">
            <img src="/images/interface/commands/command-close.png" onclick="hide_command_box('commands-boxsave')" class="command-boxclose" alt="CLOSE" />        
        </div>
        <div class="command-boxcontent">

				SAVE FORM

		</div>
        <div class="command-boxbottom">
        </div>
        </div>
<!--END SAVE BOX -->

<!--BEGIN COPY BOX -->
		<div class="commands-box" id="commands-boxcopy">
        <div class="command-boxtop">
            <img src="/images/interface/commands/command-close.png" onclick="hide_command_box('commands-boxcopy')" class="command-boxclose" alt="CLOSE" />        
        </div>
        <div class="command-boxcontent">
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
				<strong>Are you sure you want to copy this item?</strong>
                <br />
                <br />
                <br />
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Interface/btn_yes.png" ToolTip="Yes, Copy This" OnClick="CopyItem_Click" AlternateText="YES"></asp:ImageButton>
                &nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Interface/btn_no.png" ToolTip="No, Don't Copy Anything" AlternateText="NO"></asp:ImageButton>

		</div>
        <div class="command-boxbottom">
        </div>
        </div>
<!--END COPY BOX -->

<!--BEGIN ADD BOX -->
		<div class="commands-box" id="commands-boxadd">
        <div class="command-boxtop">
            <img src="/images/interface/commands/command-close.png" onclick="hide_command_box('commands-boxadd')" class="command-boxclose" alt="CLOSE" />        
        </div>
        <div class="command-boxcontent">

				ADD FORM

		</div>
        <div class="command-boxbottom">
        </div>
        </div>
<!--END ADD BOX -->

<!--BEGIN EDIT BOX -->
		<div class="commands-box" id="commands-boxedit">
        <div class="command-boxtop">
            <img src="/images/interface/commands/command-close.png" onclick="hide_command_box('commands-boxedit')" class="command-boxclose" alt="CLOSE" />        
        </div>
        <div class="command-boxcontent">

            <strong>RENAME ITEM</strong>
            <br />
            <br />
            <strong>Title:</strong> <asp:TextBox ID="txt_rename" runat="server" Width="350px" MaxLength="420"></asp:TextBox>
            <asp:RequiredFieldValidator ID="val_rename" runat="server" ControlToValidate="txt_rename" ErrorMessage="Required" ValidationGroup="Rename" ForeColor="Red">X</asp:RequiredFieldValidator>
            <br />
            <br />
            <strong>Description:</strong>
            <br />
            <asp:TextBox ID="txt_renameDescription" runat="server" Height="75px" Width="400px" TextMode="MultiLine" />
            <br />
            <br />
            <asp:Button ID="btn_rename" runat="server" Text="Rename" ValidationGroup="Rename" onclick="btn_rename_Click" Height="50px" />

		</div>
        <div class="command-boxbottom">
        </div>
        </div>      			
<!--END EDIT BOX -->


<!--BEGIN DELETE BOX -->
		<div class="commands-box" id="commands-boxdelete">
        <div class="command-boxtop">
            <img src="/images/interface/commands/command-close.png" onclick="hide_command_box('commands-boxdelete')" class="command-boxclose" alt="CLOSE" />        
        </div>
        <div class="command-boxcontent">

            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
				<strong>Are you sure you want to delete this item?</strong>
                <br />
                <br />
                <br />
                <asp:ImageButton ID="imb_delete_yes" runat="server" ImageUrl="~/Images/Interface/btn_yes.png" ToolTip="Yes, Delete This" OnClick="DeleteItem_Click" AlternateText="YES"></asp:ImageButton>
                &nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="imb_delete_no" runat="server" ImageUrl="~/Images/Interface/btn_no.png" ToolTip="No, Don't Delete Anything" AlternateText="NO"></asp:ImageButton>

		</div>
        <div class="command-boxbottom">
        </div>
        </div>
<!--END DELETE BOX -->


<!--BEGIN SHARE BOX -->
		<div class="commands-box" id="commands-boxshare">
        <div class="command-boxtop">
            <img src="/images/interface/commands/command-close.png" onclick="hide_command_box('commands-boxshare')" class="command-boxclose" alt="CLOSE" />        
        </div>
        <div class="command-boxcontent">

				SHARE FORM

		</div>
        <div class="command-boxbottom">
        </div>
        </div>
<!--END SHARE BOX -->

		<div id="commands-title"><img src="/images/interface/commands/command-title.png" alt="" /></div>

        <span id="user_command_panel" runat="server" visible="false">

		<div class="commands-public"><img src="/images/interface/commands/command-copy.png" 
			onMouseOver="this.src='/images/interface/commands/command-copyO.png';" 
			onMouseOut="this.src='/images/interface/commands/command-copy.png';" 
			onClick ="show_command_box('commands-boxcopy')" alt="" /></div>
 <!--
		<div class="commands-public"><img src="/images/interface/commands/command-fav.png" 
			onMouseOver="this.src='/images/interface/commands/command-favO.png';" 
			onMouseOut="this.src='/images/interface/commands/command-fav.png';" 
			onClick ="show_command_box('commands-boxsave')" alt="" /></div>
 -->                        
        </span>

        <span id="author_command_panel" runat="server" visible="false">
 
		<div class="commands-private" style="display:none;"><img src="/images/interface/commands/command-add.png" 
			onMouseOver="this.src='/images/interface/commands/command-addO.png';" 
			onMouseOut="this.src='/images/interface/commands/command-add.png';" 
			onClick ="show_command_box('commands-boxadd')" alt="" /></div>

		<div class="commands-private"><img src="/images/interface/commands/command-edit.png" 
			onMouseOver="this.src='/images/interface/commands/command-editO.png';" 
			onMouseOut="this.src='/images/interface/commands/command-edit.png';" 
			onClick ="show_command_box('commands-boxedit')" alt="" /></div>

		<div class="commands-private"><img src="/images/interface/commands/command-delete.png" 
			onMouseOver="this.src='/images/interface/commands/command-deleteO.png';" 
			onMouseOut="this.src='/images/interface/commands/command-delete.png';" 
			onClick ="show_command_box('commands-boxdelete')" alt="" /></div>
                   
        </span>

		<div class="commands-public"><img src="/images/interface/commands/command-share.png" 
			onMouseOver="this.src='/images/interface/commands/command-shareO.png';" 
			onMouseOut="this.src='/images/interface/commands/command-share.png';" 
			onClick ="show_command_box('commands-boxshare')" alt="" /></div>

		<img src="/images/interface/commands/command-bottom.png" alt="" />

	</div>

	<div id="comments">

		<div id="comments-top">
        
            <asp:LoginView ID="LoginView_Comments" runat="server">
        
                <AnonymousTemplate>
                
                Login to Comment...

                </AnonymousTemplate>
        
                <LoggedInTemplate>
                               
                </LoggedInTemplate>
        
            </asp:LoginView>

            <asp:TextBox ID="txt_comment" runat="server" TextMode="MultiLine" Height="50px" Width="185px" MaxLength="420"></asp:TextBox>
            <asp:RequiredFieldValidator ID="req_commentText" runat="server" ControlToValidate="txt_comment" ErrorMessage="Required" ForeColor="Red" Font-Bold="true" ValidationGroup="AddComment">X</asp:RequiredFieldValidator>
            <asp:ImageButton ID="imb_addComment" runat="server" 
                ImageUrl="/images/interface/comments/comments-submit.png" 
                onclick="imb_addComment_Click" ValidationGroup="AddComment" />        
                			
		</div>
        
        <!--BEGIN COMMENT REPEATER-->
        
        <asp:Repeater ID="Rep_Comments" runat="server" DataSourceID="Sql_Comments">
            <ItemTemplate>

		<div class="comments-content">

			<img src="/images/interface/comments/comments-line.png" alt="" />
            <a href='/<%#Eval("userName")%>'>
			<img class="comments-userpic" src='/images/Users/<%#Eval("userName")%>.jpg' width="75" alt='<%#Eval("userName")%>' />
            </a>
			<div class="comments-username"><a href='/<%#Eval("userName")%>'><%#Eval("userName")%></a></div>
			<div class="comments-text"><%# SpiralFound.Helper.Trim(Eval("itemCommentText"), 420)%></div>
			<div class="clearfloat"></div>

		</div>

            </ItemTemplate>
        </asp:Repeater>

        <!--END COMMENT REPEATER-->

		<div id="comments-title"><img src="/images/interface/comments/comments-bottom.png" alt="" /></div>

	</div>

</div>

<div id="col_center">

	<div id="book">

		<div id="item-top"></div>


        
        <asp:Repeater ID="Rep_Items" runat="server" DataSourceID="Sql_Items">
            <ItemTemplate>

            <a href='<%#Eval("itemUrl")%>' title='<%# SpiralFound.Helper.Trim(Eval("itemName"), 100)%>' target="_blank">
		    <div class="item-image"><asp:Image ID="img_item" runat="server" alt='<%# SpiralFound.Helper.Trim(Eval("itemName"), 100)%>' ImageUrl='<%# "/images/items/" + Eval("itemImage") + ".jpg" %>' Width="640" /></div>
		    <div class="item-title"><%# SpiralFound.Helper.Trim(Eval("itemName"), 40)%></div>
		    <div class="item-descrip"><%# SpiralFound.Helper.Trim(Eval("itemDescription"), 420)%></div>
            </a>
            </ItemTemplate>
        </asp:Repeater>
	
		<div class="item-bottom"></div>	

	</div>
</div>

<div id="col_right">
	<div id="similartitle">
		<img src="/images/interface/temp-similar.png" alt="" />
	</div>

	<div id="similar">
		<div id="container">
        <!--BEGIN SIMILAR REPEATER-->

        <asp:Repeater ID="Rep_Similar" runat="server" DataSourceID="Sql_Similar">
            <ItemTemplate>

			<a href='/Item/<%#Eval("itemId")%>' title='<%# SpiralFound.Helper.Trim(Eval("itemName"), 100)%>'>
			<div class="item">
				<div class="itemtop"></div>	
				<div class="itemthumb-content"><img src='/images/items/<%#Eval("itemImage")%>.jpg' width="170" alt='<%# SpiralFound.Helper.Trim(Eval("itemName"), 100)%>' /></div>	
				<div class="itemtitle"><p class="itemthumb-titletext"><%# SpiralFound.Helper.Trim(Eval("itemName"), 20)%></p></div>
				<div class="itemthumb-descrip"><p class="itemthumb-descriptext"><%# SpiralFound.Helper.Trim(Eval("itemDescription"), 75)%></p></div>
				<div class="itemthumb-bottom"></div>
			</div>
			</a>

            </ItemTemplate>
        </asp:Repeater>

        <!--END SIMILAR REPEATER-->

		</div>
	</div>
</div> 


        </ContentTemplate>

    </asp:UpdatePanel>
<!--END UPDATE PANEL -->


<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="Sql_Items" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT * FROM tbl_item WHERE itemId = @itemId">
        <SelectParameters>
            <asp:Parameter Name="itemId" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="Sql_Comments" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT * FROM tbl_item_comment, tbl_user WHERE itemId = @itemId AND tbl_item_comment.userId = tbl_user.userId ORDER BY itemCommentCreateDate DESC">
        <SelectParameters>
            <asp:Parameter Name="itemId" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="Sql_Similar" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" SelectCommand="SELECT TOP 10 * FROM tbl_item">
    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->

</asp:Content>
