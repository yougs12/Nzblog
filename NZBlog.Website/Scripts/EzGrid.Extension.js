ajaxLoad.edit = false; //默认不能直接列表上编辑
ajaxLoad.primaryField = ''; //编辑行提交的主键，根据此字段值更改数据
ajaxLoad.editMode = 'add';//编辑模式
validfunc = {};//数据验证命名空间
$.fn.extend({
	subBtn: function () {//点击提交数据
		if (ajaxLoad.edit) {
			this.show().click(function () { if (ajaxLoad.editMode == 'add') { submitAdd(); } else { submitEdit(); } });
		}
	},
	addBtn: function () {//添加默认添加数据按钮
		if (ajaxLoad.edit) {
			this.show().click(function () { ajaxLoad.GridBody.addData() });
		}
	},
	addData: function () {//新增一行编辑
		ajaxLoad.editMode = 'add';
		var newRow = ajaxLoad.startData;
		$(this).prepend(newRow);
		$(this).find('tr').first().find('td').html('');
		$(this).find('tr').first().attr('status', 'add').find('td[editType]').each(function (i) {
			var editType = $(this).attr('editType');
			var editField = $(this).attr('field');
			var defaultValue = $(this).attr('defaultValue');
			if (editType != 'select') {
				var htmlInput = '<input type="' + editType + '" id="' + editField + '" name="' + editField + '" style="width:98%;height:100%;background-color:#F6F8BD;border-color:#ccc;">';
				$(this).html(htmlInput);
				var inp = $(this).find('input');
				if (editType == 'checkbox') {
					inp.width('20').height('20');
					if (defaultValue && defaultValue == '1') {
						$(inp).get(0).checked = true;
					}
				} else if (editType == 'text') {
					if (defaultValue != undefined) {
						inp.val(defaultValue);
					}
				}
			}
		});
	},
	editData: function () {//双击编辑指定的行
		ajaxLoad.editMode = 'edit';
		var jsonRowData = ajaxLoad.jsonArray[$(this).attr('row')];
		$(this).attr('status', 'edit').find('td[editType]').each(function (i) {
			var editType = $(this).attr('editType');
			var editField = $(this).attr('field');
			var defaultValue = jsonRowData[editField];
			if (editType != 'select') {
				var htmlInput = '<input type="' + editType + '" id="' + editField + '" name="' + editField + '" style="width:98%;height:100%;background-color:#F6F8BD;border-color:#ccc;">';
				$(this).html(htmlInput);
				var inp = $(this).find('input');
				if (editType == 'checkbox') {
					inp.width('20').height('20');
					if (defaultValue && defaultValue == '1') {
						$(inp).get(0).checked = true;
					}
				} else if (editType == 'text') {
					if (defaultValue != undefined) {
						inp.val(defaultValue);
					}
				}
			}
		})
	},
	mpost: function (callback, form, isnotShow) {//明细编辑页面提交数据的方法
		this.click(function () {
			$(this).attr('disabled', true);
			var _btn = $(this);
			if (!form) form = 'form'
			ajaxLoad.validataObj = $(form);//指定数据验证的对象form
			$.mpost($(form).attr('action'), $(form).serialize(), function (data, status) {
				_btn.removeAttr('disabled');
				if (status == 0) {
					exshow(data);
				} else { callback(data); }
			}, isnotShow);
		});
	},
})

//提交添加的数据
ajaxLoad.addParm = {};
function submitAdd() {
	if (!ajaxLoad.addUrl) ajaxLoad.addUrl = ajaxLoad.editUrl;
	if (!ajaxLoad.addOkCallback) ajaxLoad.addOkCallback = ajaxLoad.editOkCallback;
	if (!ajaxLoad.addErrorCallback) ajaxLoad.addErrorCallback = ajaxLoad.editErrorCallback;
	var cnt = ajaxLoad.GridBody.find('[status=add]').length;
	ajaxLoad.GridBody.find('[status=add]').each(function (i) {
		var data = ajaxLoad.addParm;
		$(this).find('td[editType]').children().each(function () {
			var editType = $(this).attr('type');
			if (editType == 'checkbox')
				data[$(this).attr('name')] = $(this).get(0).checked;
			else
				data[$(this).attr('name')] = $(this).val();
		});
		$.post(ajaxLoad.addUrl, data, function (resultdata, status) {
			if (status == 'success') {
				if (ajaxLoad.addOkCallback) {
					var isload = ajaxLoad.addOkCallback(resultdata);
					if ((isload == undefined || isload == true) && i == (cnt - 1)) { LoadPage(1); } else { }
				}
			} else {
				if (ajaxLoad.addErrorCallback)
					ajaxLoad.addErrorCallback();
			}
		});
	});
}
ajaxLoad.editParm = {};
function submitEdit() {//提交编辑的数据
	var cnt = ajaxLoad.GridBody.find('[status=edit]').length;
	var retIndex = 0;
	ajaxLoad.editParm = ajaxLoad.addParm;
	ajaxLoad.GridBody.find('[status=edit]').each(function (i) {
		var jsonRowData = ajaxLoad.jsonArray[$(this).attr('row')];
		var data = ajaxLoad.editParm;
		data[ajaxLoad.primaryField] = jsonRowData[ajaxLoad.primaryField];
		$(this).find('td[editType]').children().each(function () {
			var editType = $(this).attr('type');
			if (editType == 'checkbox')
				data[$(this).attr('name')] = $(this).get(0).checked;
			else
				data[$(this).attr('name')] = $(this).val();
		});
		$.post(ajaxLoad.editUrl, data, function (resultdata, status) {
			retIndex++;
			if (status == 'success') {
				if (ajaxLoad.editOkCallback) {
					var isload = ajaxLoad.editOkCallback(resultdata);
					if ((isload == undefined || isload == true) && retIndex == cnt) { LoadPage(1); } else { }
				}
			} else {
				if (ajaxLoad.editErrorCallback)
					ajaxLoad.editErrorCallback();
			}
		});
	});
}
ajaxLoad.editErrorCallback = function () {
	exshow();
}

ajaxLoad.validata = function () {//验证数据
	var _msg = '';
	ajaxLoad.validataObj.find('[valid]').each(function () {
		var _value = $(this).trim();
		var _valiFuncName = $(this).attr('valid');
		if (_value == undefined || _valiFuncName == undefined) return;
		for (var _v in validfunc) {
			if (_v == _valiFuncName) {
				var _func = validfunc[_v];
				var _result = _func(_value);
				if (_result != undefined && _result != '') _msg += _result + '<br>';
			}
		}
	});
	ajaxLoad.validataObj.find('[notnull]').each(function () {
		var valitype = $(this).attr('valid');
		var alert = $(this).attr('alert');
		var _title = $(this).attr('title');
		if (!_title) _title = ''; else _title = '“' + _title + '”';
		var _value = $(this).trim();
		if (_value == undefined || _value == '') {
			if (alert && alert != '') _msg += alert; else _msg += '请将必填项' + _title + '填写完整！<br>';
		}
	});
	ajaxLoad.validataObj.find('[lmax]').each(function () {
		var maxlength = $(this).attr('lmax');
		var alert = $(this).attr('alert');
		var _title = $(this).attr('title');
		if (!_title) _title = ''; else _title = '“' + _title + '”';
		var _value = $(this).trim();
		if (_value == undefined || _value.length > maxlength) {
			if (alert && alert != '') _msg += alert; else _msg += _title + '长度不能大于' + maxlength + '！<br>';
		}
	});
	ajaxLoad.validataObj.find('[maxNum]').each(function () {
		var maxlength = $(this).attr('maxNum');
		var alert = $(this).attr('alert');
		var _title = $(this).attr('title');
		if (!_title) _title = ''; else _title = '“' + _title + '”';
		var _value = $(this).trim();
		_value = parseFloat(_value);
		if (_value == undefined || _value > maxlength) {
			if (alert && alert != '') _msg += alert; else _msg += _title + '不能大于' + maxlength + '！<br>';
		}
	});
	ajaxLoad.validataObj.find('[lmin]').each(function () {
		var minlength = $(this).attr('lmin');
		var alert = $(this).attr('alert');
		var _title = $(this).attr('title');
		if (!_title) _title = ''; else _title = '“' + _title + '”';
		var _value = $(this).trim();
		if (_value == undefined || _value.length < minlength) {
			if (alert && alert != '') _msg += alert; else _msg += _title + '长度不能小于' + minlength + '！<br>';
		}
	});
	ajaxLoad.validataObj.find('[minNum]').each(function () {
		var minlength = $(this).attr('minNum');
		var alert = $(this).attr('alert');
		var _title = $(this).attr('title');
		if (!_title) _title = ''; else _title = '“' + _title + '”';
		var _value = $(this).trim();
		_value = parseFloat(_value);
		if (_value == undefined || _value < minlength) {
			if (alert && alert != '') _msg += alert; else _msg += _title + '不能小于' + minlength + '！<br>';
		}
	});
	return _msg;
}