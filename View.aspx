<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="View.aspx.vb" Inherits="A.View" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CphContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:UpdatePanel ID="udpPanelMain" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="x_panel">
                <div class="x_title">
                    <h2>View Data</h2>
                    <div class="clearfix"></div>
                </div>
                <div class="x_content">
                 <asp:Panel ID="PnlHeader" runat="server">
                    <table class="table-condensed">
                        <tr>
                                <td>Nama
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNama" runat="server" onfocus="blur()"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RFVNama" runat="server" ControlToValidate="txtNama"
                                        Display="Dynamic" ErrorMessage="*" ForeColor="Red" ValidationGroup="View" />
                                    <asp:Label ID="lbNama" runat="server"></asp:Label>
                                </td>
                            </tr>
						</tr>
                            <td>Nomor Telpon</td>
                            <td>
                                <asp:TextBox ID="txttelp" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                            
                            <td><asp:Label ID="lbltelp1" runat="server" Visible="true" ></asp:Label></td>
                        
                        </tr>
                    
                        </tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txttelp2" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                            
                            <td><asp:Label ID="lbltelp2" runat="server" Visible="true" ></asp:Label></td>
                        
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btmSubmit" runat="server" Text="Submit" CssClass="btn-success"/>
                            </td>
                        </tr>

                        <tr>
                            <td>&nbsp;</td>
                        </tr>
						
						
						<tr>
                            <td colspan="2">
                                <asp:Button ID="btmView" runat="server" Text="Tampilkan" CssClass="btn-success"/>
                            </td>
                        </tr>

                    </table>

                    
					<table style="width: 100%">
                        <tr>
                            <td colspan="2">
                                <div id="Grid" runat="server">
                                    <asp:GridView ID="grdViewMain" runat="server" AllowPaging="true" 
                                        EmptyDataText="Data tidak ditemukan !" PageSize="5" 
                                        ShowHeaderWhenEmpty="True" Width="100%" CssClass="table table-hover" HeaderStyle-BorderStyle="Inset">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="#" HeaderText="#" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <HeaderTemplate>
                                                    Action
                                                </HeaderTemplate>
                                             
											 <HeaderStyle Height="30px" Width="80px" />
                                                <ItemStyle Height="20px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
							<tr>
                            <td colspan="2">
                                <asp:Button ID="btmExport" runat="server" Text="Export" CssClass="btn-success"/>
                            </td>
                        </tr>
                            </td>
                        </tr>
                    </table>
            </asp:Panel>
                </div>
             </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
