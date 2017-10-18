/// <reference path="~/Scripts/jQuery/jquery-1.6.3.js" />

/* Helper methods for Auto Complete feature */



function initializeClientAutoComplete(options) {
    var parameter = jQuery.parseJSON(options);
    var txtClient = jQuery('#' + parameter.Client);
    var txtClientId = jQuery('#' + parameter.ClientId);
    var btnSelectClient = jQuery('#' + parameter.SelectClient);
    var UserId = parameter.UserId;

    txtClient.autocomplete('destroy');
    txtClient.autocomplete(
                {
                    minLength: 0,
                    source: function (request, response) {
                        $.ajax(
                            {
                                url: '../WebServices/MasterService.asmx/GetClientsByName',
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                data: "{ 'name': '" + request.term + "','userid': '" + UserId + "'}",
                                dataType: "json",
                                success: function (data, textStatus, req) {
                                    var dataArray = $.parseJSON(data.d);
                                    if (dataArray === null) return;
                                    response(dataArray);
                                },
                                error: function (req, textStatus, error) {
                                    alert(error);
                                }
                            });
                    },

                    focus: function (event, ui) {
                        $(this).val(ui.item.Name);
                        return false;
                    },

                    select: function (event, ui) {
                        $(this).val(ui.item.Name);
                        txtClientId.val(ui.item.Id);
                        return false;
                    },

                    change: function (event, ui) {
                        if (ui.item === null) {
                            $(this).val("");
                            txtClientId.val("0");
                        }
                        btnSelectClient.click();
                        return false;
                    }
                })
                .data("autocomplete")._renderItem = function (ul, item) {
                    return $("<li></li>")
				            .data("item.autocomplete", item)
                    //                            .append("<a><span>" + item.Name + "<br>" + item.Description + "</span></a>")
                            .append("<a><span>" + item.Name + "</span></a>")
				            .appendTo(ul);
                };
}

function initializeProjectAutoComplete(options) {
    var parameter = jQuery.parseJSON(options);
    var txtProject = jQuery('#' + parameter.Project);
    var txtProjectId = jQuery('#' + parameter.ProjectId);
    var txtClientId = jQuery('#' + parameter.ClientId);
    var btnSelectProject = jQuery('#' + parameter.SelectProject);
    var UserId = parameter.UserId;

    txtProject.autocomplete('destroy');
    txtProject.autocomplete(
                {
                    minLength: 0,
                    source: function (request, response) {
                        $.ajax(
                            {
                                url: '../WebServices/MasterService.asmx/GetProjectsByClient',
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                data: "{ 'clientId': '" + txtClientId.val() + "','userid': '" + UserId + "'}",
                                dataType: "json",
                                success: function (data, textStatus, req) {
                                    var dataArray = $.parseJSON(data.d);
                                    if (dataArray === null) return;
                                    response(dataArray);
                                },
                                error: function (req, textStatus, error) {
                                    alert(error);
                                }
                            });
                    },

                    focus: function (event, ui) {
                        $(this).val(ui.item.Name);
                        return false;
                    },

                    select: function (event, ui) {
                        $(this).val(ui.item.Name);
                        txtProjectId.val(ui.item.Id);
                        return false;
                    },

                    change: function (event, ui) {
                        if (ui.item === null) {
                            $(this).val("");
                            txtProjectId.val("0");
                        }
                        btnSelectProject.click();
                        return false;
                    }
                })
                .data("autocomplete")._renderItem = function (ul, item) {
                    return $("<li></li>")
				            .data("item.autocomplete", item)
                    //                            .append("<a><span>" + item.Name + "<br>" + item.Description + "</span></a>")
                    .append("<a><span>" + item.Name + "</span></a>")
				            .appendTo(ul);
                };
}

function initializeLanguageAutoComplete(options) {
    var parameter = jQuery.parseJSON(options);
    var txtLanguage = jQuery('#' + parameter.Language);
    var txtLangaugeId = jQuery('#' + parameter.LanguageId);

    txtLanguage.autocomplete('destroy');
    txtLanguage.autocomplete(
                {
                    minLength: 0,
                    source: function (request, response) {
                        $.ajax(
                            {
                                url: '../WebServices/MasterService.asmx/GetLanguagesByName',
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                data: "{ 'name': '" + request.term + "'}",
                                dataType: "json",
                                success: function (data, textStatus, req) {
                                    var dataArray = $.parseJSON(data.d);
                                    if (dataArray === null) return;
                                    response(dataArray);
                                },
                                error: function (req, textStatus, error) {
                                    alert(error);
                                }
                            });
                    },

                    focus: function (event, ui) {
                        $(this).val(ui.item.Name);
                        return false;
                    },

                    select: function (event, ui) {
                        $(this).val(ui.item.Name);
                        txtLangaugeId.val(ui.item.Id);
                        return false;
                    },

                    change: function (event, ui) {
                        if (ui.item === null) {
                            $(this).val("");
                            txtLangaugeId.val("0");
                        }
                        return false;
                    }
                })
                .data("autocomplete")._renderItem = function (ul, item) {
                    return $("<li></li>")
				            .data("item.autocomplete", item)
                    //                            .append("<a><span>" + item.Name + "<br>" + item.Description + "</span></a>")
                    .append("<a><span>" + item.Name + "</span></a>")
				            .appendTo(ul);
                };
}
function initializeLocationAutoComplete(options) {
    var parameter = jQuery.parseJSON(options);
    var txtLocation = jQuery('#' + parameter.Location);
    var txtLocationId = jQuery('#' + parameter.LocationId);
    var UserId = parameter.UserId;

    txtLocation.autocomplete('destroy');
    txtLocation.autocomplete(
                {
                    minLength: 0,
                    source: function (request, response) {
                        $.ajax(
                            {
                                url: '../WebServices/MasterService.asmx/GetLocationsByCity',
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                data: "{ 'cityName': '" + request.term + "','userid': '" + UserId + "'}",
                                dataType: "json",
                                success: function (data, textStatus, req) {
                                    var dataArray = $.parseJSON(data.d);
                                    if (dataArray === null) return;
                                    response(dataArray);
                                },
                                error: function (req, textStatus, error) {
                                    alert(error);
                                }
                            });
                    },

                    focus: function (event, ui) {
                        $(this).val(ui.item.City);
                        return false;
                    },

                    select: function (event, ui) {
                        $(this).val(ui.item.City);
                        txtLocationId.val(ui.item.Id);
                        return false;
                    },

                    change: function (event, ui) {
                        if (ui.item === null) {
                            $(this).val("");
                            txtLocationId.val("0");
                        }
                        return false;
                    }
                })
                .data("autocomplete")._renderItem = function (ul, item) {
                    return $("<li></li>")
				            .data("item.autocomplete", item)
                    //                            .append("<a><span>" + item.Name + "<br>" + item.Description + "</span></a>")
                    .append("<a><span>" + item.City + "(" + item.Country + ")</span></a>")
				            .appendTo(ul);
                };
}















