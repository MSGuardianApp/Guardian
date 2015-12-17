<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateGroup.aspx.cs" Inherits="SOS.Web.CreateGroup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="Scripts/js/jquery-1.9.0.js"></script>
<head runat="server">
    <title></title>
    <style type="text/css">
        input, textarea, select {
            font: small/1.5 "Tahoma", "Bitstream Vera Sans", Verdana, Helvetica, sans-serif;
        }

        table, td {
            border: 1px solid #CCC;
            border-collapse: collapse;
            font: small/1.5 "Tahoma", "Bitstream Vera Sans", Verdana, Helvetica, sans-serif;
        }

        table {
            border: none;
            border: 1px solid #CCC;
        }

        thead th, tbody th {
            background: #FFF url(th_bck.gif) repeat-x;
            color: #666;
            padding: 5px 10px;
            border-left: 1px solid #CCC;
        }

        tbody th {
            background: #fafafb;
            border-top: 1px solid #CCC;
            text-align: left;
            font-weight: normal;
        }

        tbody tr td {
            padding: 5px 10px;
            color: #666;
        }

        tbody tr:hover {
            background: #FFF url(tr_bck.gif) repeat;
        }

            tbody tr:hover td {
                color: #454545;
            }

        tfoot td, tfoot th {
            border-left: none;
            border-top: 1px solid #CCC;
            padding: 4px;
            background: #FFF url(foot_bck.gif) repeat;
            color: #666;
        }

        table a:link {
            color: #666;
        }

        table a:visited {
            color: #666;
        }

        table a:hover {
            color: #003366;
            text-decoration: none;
        }

        table a:active {
            color: #003366;
        }

        span {
            padding-left: 2px;
            padding-right: 2px;
        }

        .truncate {
            width: 75px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .FileUploadHidden {
            display: none;
        }

        .modal-backdrop {
            background-color: rgba(0, 0, 0, 0.61);
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            display: none;
        }

        .modal {
            width: 500px;
            position: absolute;
            top: 25%;
            z-index: 1020;
            background-color: #FFF;
            border-radius: 6px;
            display: none;
        }

        .modal-header {
            background-color: #333;
            color: #FFF;
            border-top-right-radius: 5px;
            border-top-left-radius: 5px;
        }

            .modal-header h3 {
                margin: 0;
                padding: 0 10px 0 10px;
                line-height: 40px;
            }

                .modal-header h3 .close-modal {
                    float: right;
                    text-decoration: none;
                    color: #FFF;
                }

        .modal-footer {
            background-color: #F1F1F1;
            padding: 0 10px 0 10px;
            line-height: 40px;
            text-align: right;
            border-bottom-right-radius: 5px;
            border-bottom-left-radius: 5px;
            border-top: solid 1px #CCC;
        }

        .modal-body {
            padding: 10px 10px 10px 10px;
        }
    </style>

    <script>
        $(function () {
            modalPosition();
            $(window).resize(function () {
                modalPosition();
            });
            $('.openModal').click(function (e) {
                $('.modal, .modal-backdrop').fadeIn('fast');
                e.preventDefault();
            });
            $('.close-modal').click(function (e) {
                $('.modal, .modal-backdrop').fadeOut('fast');
            });
        });
        function modalPosition() {
            var width = $('.modal').width();
            var pageWidth = $(window).width();
            var x = (pageWidth / 2) - (width / 2);
            $('.modal').css({ left: x + "px" });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <div>
            <div style="border-top-color: #ED7D31; border-top-width: 4px; border-top-style: solid;"></div>
            <div class="fragment Default">
                <div class="header Banner">
                    <h2>Groups Management </h2>
                    <div style="text-align: right">
                        <a href="Settings.aspx">Admin Features</a>
                    </div>
                </div>
                <div style="border-top-color: green; border-top-width: 4px; border-top-style: solid; padding-bottom: 5px;"></div>
            </div>
            <div class="datatable">
                <asp:HiddenField ID="HdnFileName" runat="server" />
                <asp:ListView ID="lvwgroup" runat="server" DataKeyNames="GroupID" OnItemCommand="lvwgroup_ItemCommand" OnItemInserting="lvwgroup_ItemInserting"
                    OnItemEditing="lvwgroup_ItemEditing" OnItemUpdating="lvwgroup_ItemUpdating" OnPagePropertiesChanging="lvwgroup_PagePropertiesChanging"
                    OnItemCanceling="lvwgroup_ItemCanceling" OnItemDeleting="lvwgroup_ItemDeleting">
                    <LayoutTemplate>
                        <table style="width: 100%;">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>Group Name</th>
                                    <th>Domain</th>
                                    <th>Location</th>
                                    <th>Email</th>
                                    <th>Phone Number</th>
                                    <th>Group Type</th>
                                    <th>Is Active</th>
                                    <th>Parent Group Name</th>
                                </tr>
                            </thead>
                            <tbody id="itemplaceholder" runat="server"></tbody>
                            <tfoot>
                                <tr>
                                    <th style="text-align: left">
                                        <asp:LinkButton ID="NewButton" runat="server" Text="New" OnClick="NewButton_Click" CommandName="New"></asp:LinkButton>
                                    </th>
                                    <th colspan="13" style="text-align: right">
                                        <asp:DataPager runat="server" ID="dpGroups" PageSize="10" PagedControlID="lvwgroup">
                                            <Fields>
                                                <asp:NumericPagerField ButtonCount="5" />
                                            </Fields>
                                        </asp:DataPager>
                                    </th>
                                </tr>
                            </tfoot>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="DeleteButton" CommandName="Delete" CommandArgument='<%# Eval("GroupID")%>' runat="server" Text="Delete"></asp:LinkButton>
                                <asp:LinkButton ID="EditButton" CommandName="Edit" runat="server" Text="Edit"></asp:LinkButton>
                                <asp:HiddenField ID="PartitionKey" runat="server" Value='<%# Eval("PartitionKey")%>' />
                                <asp:HiddenField ID="RowKey" runat="server" Value='<%# Eval("RowKey")%>' />
                            </td>
                            <td>
                                <asp:Label ID="GrpName" runat="server" Text='<% #Eval("GroupName")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="EnrollmentKey" runat="server" Text='<% #Eval("EnrollmentKey")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Location" runat="server" Text='<% #Eval("Location")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Email" runat="server" Text='<% #Eval("Email")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="PhoneNumber" runat="server" Text='<% #Eval("PhoneNumber")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="GroupType" runat="server" Text='<% #Eval("GroupType")%>' Width="20px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="IsActive" runat="server" Text='<% #Eval("IsActive")%>' Width="10px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="ParentGroupID" runat="server" Text='<% #Eval("ParentGroupName")%>'></asp:Label>
                            </td>


                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="odd">
                            <td>
                                <asp:LinkButton ID="DeleteButton" CommandName="Delete" CommandArgument='<%# Eval("GroupID")%>' runat="server" Text="Delete"></asp:LinkButton>
                                <asp:LinkButton ID="EditButton" CommandName="Edit" runat="server" Text="Edit"></asp:LinkButton>
                                <asp:HiddenField ID="PartitionKey" runat="server" Value='<%# Eval("PartitionKey")%>' />
                                <asp:HiddenField ID="RowKey" runat="server" Value='<%# Eval("RowKey")%>' />
                            </td>
                            <td>
                                <asp:Label ID="GrpName" runat="server" Text='<% #Eval("GroupName")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="EnrollmentKey" runat="server" Text='<% #Eval("EnrollmentKey")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Location" runat="server" Text='<% #Eval("Location")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Email" runat="server" Text='<% #Eval("Email")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="PhoneNumber" runat="server" Text='<% #Eval("PhoneNumber")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="GroupType" runat="server" Text='<% #Eval("GroupType").ToString()%>' Width="20px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="IsActive" runat="server" Text='<% #Eval("IsActive")%>' Width="10px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="ParentGroupID" runat="server" Text='<% #Eval("ParentGroupName")%>'></asp:Label>
                            </td>

                        </tr>
                    </AlternatingItemTemplate>
                    <EditItemTemplate>
                        <tr>
                            <td colspan="14">
                                <div style="padding: 10px">
                                    <span style="align-content: center; font-family: 'Segoe UI'; font-weight: bold; text-decoration: underline; font-size: 14px">Edit Group</span>
                                    <asp:HiddenField ID="PartitionKey" runat="server" Value='<%# Eval("PartitionKey")%>' />
                                    <asp:HiddenField ID="RowKey" runat="server" Value='<%# Eval("RowKey")%>' />
                                    <table border="0" style="width: 80%">
                                        <tbody>
                                            <tr>
                                                <th>Group Name:
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="GroupNameTextBox" runat="server" Text='<% #Bind("GroupName")%>'></asp:TextBox>
                                                </td>

                                                <th>Parent Group Name:
                                                </th>
                                                <td>
                                                    <asp:DropDownList ID="ddlParentGroupNames" runat="server" DataTextField="GroupName" DataValueField="GroupID" OnSelectedIndexChanged="ddlParentGroupNames_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Email:
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="EmailTextBox" runat="server" Text='<% #Bind("Email")%>'></asp:TextBox>
                                                </td>

                                                <th>Phone Number:
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="PhoneNumberTextBox" runat="server" Text='<% #Bind("PhoneNumber")%>'></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Location:
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="LocationTextBox" runat="server" ReadOnly="true" Text='<% #Bind("Location")%>'></asp:TextBox>
                                                </td>

                                                <th>Focus Lat/Long:</th>
                                                <td>
                                                    <asp:TextBox ID="GeoLocationTextBox" runat="server" Text='<% #Bind("GeoLocation")%>' Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <th>Group Type :
                                                </th>
                                                <td>
                                                    <asp:RadioButton ID="rGroupType1" GroupName="GroupType" Text="Private" Checked="true" runat="server" />
                                                    <asp:RadioButton ID="rGroupType2" GroupName="GroupType" Text="Public" runat="server" />
                                                    <asp:RadioButton ID="rGroupType3" GroupName="GroupType" Text="Social" runat="server" />
                                                </td>                                             
                                            </tr>
                                            <tr>
                                                <th>Domain/Key:
                                                </th>
                                                <td  colspan="3">
                                                    <asp:TextBox ID="EnrollmentKeyTextBox" runat="server" Text='<% #Bind("EnrollmentKey")%>'></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <th>Live UserID:
                                                </th>
                                                <td colspan="3">
                                                    <asp:TextBox ID="LiveIDTextBox" runat="server" Text='<% #Bind("LiveID")%>' Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <th>Shape Files:
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="ShapeFileIDTextBox" runat="server" Text='<% #Bind("ShapeFileID")%>'></asp:TextBox>
                                                    <asp:LinkButton ID="uploadButton" runat="server" Text="Upload" CssClass="openModal"></asp:LinkButton>
                                                </td>
                                                <th>Circle Key:
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="GroupKeyTextBox" runat="server" Text='<% #Bind("GroupKey")%>'></asp:TextBox>
                                                    <asp:DropDownList ID="CircleKeyDropDownList" runat="server"></asp:DropDownList>
                                                </td>

                                            </tr>

                                            <tr>
                                                <th>Settings:</th>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="NotifySubgroupsCheckBox" runat="server" Text="Notify SubGroups" Checked='<% #Bind("NotifySubgroups")%>'></asp:CheckBox>
                                                    &nbsp;
                                                <asp:CheckBox ID="AllowGroupManagementCheckBox" runat="server" Text="AllowGroupManagement" Checked='<% #Bind("AllowGroupManagement")%>'></asp:CheckBox>
                                                    &nbsp;
                                                <asp:CheckBox ID="ShowIncidentsCheckBox" runat="server" Text="ShowIncidents" Checked='<% #Bind("ShowIncidents")%>'></asp:CheckBox>
                                                    &nbsp;
                                                <asp:CheckBox ID="IsActiveCheckBox" runat="server" Text="IsActive" Checked='<% #Bind("IsActive")%>'></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th></th>
                                                <td colspan="3">
                                                    <asp:LinkButton ID="UpdateButton" CommandName="Update" CommandArgument='<%# Eval("GroupID")%>' runat="server" Text="Update"></asp:LinkButton>
                                                    <asp:LinkButton ID="CancelButton" CommandName="Cancel" runat="server" Text="Cancel"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>

                            </td>
                        </tr>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <tr>
                            <td colspan="14">
                                <table border="0" style="width: 80%">
                                    <tbody>
                                        <tr>
                                            <th>Group Name:
                                            </th>
                                            <td>
                                                <asp:TextBox ID="GroupNameTextBox" runat="server" Text='<% #Bind("GroupName")%>'></asp:TextBox>
                                            </td>

                                            <th>Parent Group Name:
                                            </th>
                                            <td>
                                                <asp:DropDownList ID="ddlParentGroupNames" runat="server" DataTextField="GroupName" DataValueField="GroupID" OnSelectedIndexChanged="ddlParentGroupNames_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Email:
                                            </th>
                                            <td>
                                                <asp:TextBox ID="EmailTextBox" runat="server" Text='<% #Bind("Email")%>'></asp:TextBox>
                                            </td>

                                            <th>Phone Number:
                                            </th>
                                            <td>
                                                <asp:TextBox ID="PhoneNumberTextBox" runat="server" Text='<% #Bind("PhoneNumber")%>'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Location:
                                            </th>
                                            <td>
                                                <asp:TextBox ID="LocationTextBox" runat="server" Text='<% #Bind("Location")%>'></asp:TextBox>
                                            </td>

                                            <th>Focus Lat/Long:</th>
                                            <td>
                                                <asp:TextBox ID="GeoLocationTextBox" runat="server" Text='<% #Bind("GeoLocation")%>' Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Group Type :
                                            </th>
                                            <td>
                                                <asp:RadioButton ID="rGroupType1" GroupName="GroupType" Text="Private" Checked="true" runat="server" />
                                                <asp:RadioButton ID="rGroupType2" GroupName="GroupType" Text="Public" runat="server" />
                                                <asp:RadioButton ID="rGroupType3" GroupName="GroupType" Text="Social" runat="server" />

                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <th>Domain/Key:
                                            </th>
                                            <td  colspan="3">
                                                <asp:TextBox ID="EnrollmentKeyTextBox" runat="server" Text='<% #Bind("EnrollmentKey")%>'></asp:TextBox>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <th>Live UserID:
                                            </th>
                                            <td colspan="3">
                                                <asp:TextBox ID="LiveIDTextBox" runat="server" Text='<% #Bind("LiveID")%>' Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Shape Files:
                                            </th>
                                            <td>
                                                <asp:TextBox ID="ShapeFileIDTextBox" runat="server" Text='<% #Bind("ShapeFileID")%>'></asp:TextBox>
                                                <asp:LinkButton ID="uploadButton" runat="server" Text="Upload" CssClass="openModal"></asp:LinkButton>
                                            </td>
                                            <th>Circle Key:
                                            </th>
                                            <td>
                                                <asp:TextBox ID="GroupKeyTextBox" runat="server" Text='<% #Bind("GroupKey")%>'></asp:TextBox>
                                            
                                                <asp:DropDownList ID="CircleKeyDropDownList" runat="server"></asp:DropDownList>
                                            </td>                                      
                                        </tr>

                                         <tr>
                                                <th>Settings:</th>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="NotifySubgroupsCheckBox" runat="server" Text="Notify SubGroups"></asp:CheckBox>
                                                    &nbsp;
                                                <asp:CheckBox ID="AllowGroupManagementCheckBox" runat="server" Text="AllowGroupManagement"></asp:CheckBox>
                                                    &nbsp;
                                                <asp:CheckBox ID="ShowIncidentsCheckBox" runat="server" Text="ShowIncidents"></asp:CheckBox>
                                                    &nbsp;
                                                <asp:CheckBox ID="IsActiveCheckBox" runat="server" Text="IsActive" Checked="true"></asp:CheckBox>
                                                </td>
                                            </tr>
                                        <tr>
                                            <th></th>
                                            <td colspan="3">
                                                <asp:LinkButton ID="UpdateButton" CommandName="Insert" CommandArgument='<%# Eval("GroupID")%>' runat="server" Text="Insert"></asp:LinkButton>
                                                <asp:LinkButton ID="CancelButton" CommandName="Cancel" runat="server" Text="Cancel"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </InsertItemTemplate>
                </asp:ListView>
                <div style="color: red; font-weight: bold; margin-top: 5px" align="center">
                    <asp:Label ID="lblSuccess" runat="server"></asp:Label><br />
                    <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh" />
                </div>

                <div class="modal">
                    <div class="modal-header">
                        <h3>Group Shape File Upload<a class="close-modal" href="#">&times;</a></h3>
                    </div>
                    <div class="modal-body">
                        <span>Shape File (*.shp) : </span>
                        <asp:FileUpload ID="ShapeFileIDUpload" runat="server" Height="30px" Width="100%" />
                        <span>Describe File (*.dbf) : </span>
                        <asp:FileUpload ID="FileUploadDescribe" runat="server" Height="30px" Width="100%" />
                        <span>Projection File (*.prj) : </span>
                        <asp:FileUpload ID="FileUploadProjection" runat="server" Height="30px" Width="100%" />
                        <span>Shape Index (*.shx) : </span>
                        <asp:FileUpload ID="FileUploadShapeIndex" runat="server" Height="30px" Width="100%" />

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_close" runat="server" Text="Upload" CssClass="modalOK close-modal" OnClick="btn_Upload_Click" />
                    </div>
                </div>

                <div class="modal-backdrop"></div>
            </div>
        </div>
    </form>
</body>
</html>
