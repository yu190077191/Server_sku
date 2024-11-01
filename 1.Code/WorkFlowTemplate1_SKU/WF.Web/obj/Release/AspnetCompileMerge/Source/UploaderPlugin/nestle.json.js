// From: http://www.overset.com/2008/04/11/mark-gibsons-json-jquery-updated/

(function ($) {
    m = {
        '\b': '\\b',
        '\t': '\\t',
        '\n': '\\n',
        '\f': '\\f',
        '\r': '\\r',
        '"': '\\"',
        '\\': '\\\\'
    },
	$.toJSON = function (value, whitelist) {
	    var a,          // The array holding the partial texts.
			i,          // The loop counter.
			k,          // The member key.
			l,          // Length.
			r = /["\\\x00-\x1f\x7f-\x9f]/g,
			v;          // The member value.

	    switch (typeof value) {
	        case 'string':
	            return r.test(value) ?
				'"' + value.replace(r, function (a) {
				    var c = m[a];
				    if (c) {
				        return c;
				    }
				    c = a.charCodeAt();
				    return '\\u00' + Math.floor(c / 16).toString(16) + (c % 16).toString(16);
				}) + '"' :
				'"' + value + '"';

	        case 'number':
	            return isFinite(value) ? String(value) : 'null';

	        case 'boolean':
	        case 'null':
	            return String(value);

	        case 'object':
	            if (!value) {
	                return 'null';
	            }
	            if (typeof value.toJSON === 'function') {
	                return $.toJSON(value.toJSON());
	            }
	            a = [];
	            if (typeof value.length === 'number' &&
					!(value.propertyIsEnumerable('length'))) {
	                l = value.length;
	                for (i = 0; i < l; i += 1) {
	                    a.push($.toJSON(value[i], whitelist) || 'null');
	                }
	                return '[' + a.join(',') + ']';
	            }
	            if (whitelist) {
	                l = whitelist.length;
	                for (i = 0; i < l; i += 1) {
	                    k = whitelist[i];
	                    if (typeof k === 'string') {
	                        v = $.toJSON(value[k], whitelist);
	                        if (v) {
	                            a.push($.toJSON(k) + ':' + v);
	                        }
	                    }
	                }
	            } else {
	                for (k in value) {
	                    if (typeof k === 'string') {
	                        v = $.toJSON(value[k], whitelist);
	                        if (v) {
	                            a.push($.toJSON(k) + ':' + v);
	                        }
	                    }
	                }
	            }
	            return '{' + a.join(',') + '}';
	    }
	};
})(jQuery);

jQuery.extend({
    evalJSON: function (strJson) {
        return eval("(" + strJson + ")");
    }
});

jQuery.extend({
    toJSONString: function (object) {
        if (object == null) {
            return object;
        }
        var type = typeof object;
        if ('object' == type) {
            if (Array == object.constructor) type = 'array';
            else if (RegExp == object.constructor) type = 'regexp';
            else type = 'object';
        }
        switch (type) {
            case 'undefined':
            case 'unknown':
                return;
                break;
            case 'function':
            case 'boolean':
            case 'regexp':
                return object.toString();
                break;
            case 'number':
                return isFinite(object) ? object.toString() : 'null';
                break;
            case 'string':
                return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g, function () {
                    var a = arguments[0];
                    return (a == '\n') ? '\\n' : (a == '\r') ? '\\r' : (a == '\t') ? '\\t' : ""
                }) + '"';
                break;
            case 'object':
                if (object === null) return 'null';
                var results = [];
                for (var property in object) {
                    var value = jQuery.toJSONString(object[property]);
                    if (value !== undefined) results.push(jQuery.toJSONString(property) + ':' + value);
                }
                return '{' + results.join(',') + '}';
                break;
            case 'array':
                var results = [];
                for (var i = 0; i < object.length; i++) {
                    var value = jQuery.toJSONString(object[i]);
                    if (value !== undefined) results.push(value);
                }
                return '[' + results.join(',') + ']';
                break;
        }
    }
});