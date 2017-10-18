// Javascripts used in TKS application.
// Author: Prakash R

/// <reference name="MicrosoftAjax.debug.js" />
/// <reference path="jQuery/jquery-1.6.3.js" />

/* 
Class: WebDialog
Description: Used to display inline panel element as modal dialog when user want to search data.
*/
var WebDialog = function (options) {
    this._options = options;
    this._inputControlId = "";
    this._searchButtonId = "";
    this._valueControlId = "";
    this._searchControlPanelId = "";
    this._title = "";
    this._defaultValueText = "";

    this._initialize();
    this._initializeDialog();
}

WebDialog.prototype = {
    _initialize: function () {
        if (this._options === null) return;

        var _this = this;

        // Get values from options.
        if (this._options.inputControlId)
            this._inputControlId = "#" + this._options.inputControlId;
        if (this._options.searchButtonId)
            this._searchButtonId = "#" + this._options.searchButtonId;
        if (this._options.valueControlId)
            this._valueControlId = "#" + this._options.valueControlId;
        if (this._options.searchControlPanelId)
            this._searchControlPanelId = "#" + this._options.searchControlPanelId;
        if (this._options.title)
            this._title = this._options.title;
        if (this._options.defaultValueText)
            this._defaultValueText = this._options.defaultValueText;       

        // User presses F2 key then click image control.
        $(this._inputControlId).keydown(
                    function (e) {
                        var keyCode = $.ui.keyCode;
                        // F2
                        if (e.keyCode === 113) {
                            e.preventDefault();
                            $(_this._searchButtonId).click();
                        }
                        // User removes value in input control then clear display text and value text.
                        else if (e.keyCode === keyCode.BACKSPACE || e.keyCode === keyCode.DELETE) {
                            e.preventDefault();
                            _this.set_displayText("");
                            _this.set_valueText(_this.defaultValueText);
                        }
                    });
    },

    _initializeDialog: function () {
        var _this = this;
        // Initialize jQuery dialog.
        if (this._searchControlPanelId === "#") return;

        $(this._searchControlPanelId).dialog('destroy');
        $(this._searchControlPanelId).dialog(
                    {
                        autoOpen: false,
                        modal: true,
                        show: 'fade',
                        hide: 'clip',
                        draggable: true,
                        resizable: false,
                        width: '750px',
                        title: this._title,
                        open: function (event, ui) {
                            $(this).parent().appendTo("form:first");
                        },
                        close: function (event, ui) {
                            // Set the focus on input control.
                            $(_this._inputControlId).focus();
                        }
                    });
    },


    // Public members.
    get_options: function () { return this._options; },
    set_options: function (value) {
        this._options = value;
        this._initialize();
    },

    get_displayText: function () { return $(this._inputControlId).val(); },
    set_displayText: function (value) { $(this._inputControlId).val(value); },

    get_valueText: function () { return $(this._valueControlId).val(); },
    set_valueText: function (value) { $(this._valueControlId).val(value); },

    show: function () {
        $(this._searchControlPanelId).dialog('open');
    },

    close: function () {
        $(this._searchControlPanelId).dialog('close');
    }
}