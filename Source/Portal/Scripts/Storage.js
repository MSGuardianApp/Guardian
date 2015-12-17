  function checksupport() {
            if (sessionStorage)
                return true;
            else
                return false;
  }

  function setSession(key, value) {

            if (!checksupport())
                return handleerror('Error!!! No Support');

            if (key == null || key == undefined || key.toString() == '')
                return false;

            try {

                sessionStorage.setItem(key, JSON.stringify(value));
            } catch (e) {
                if (e == 'QUOTA_EXCEEDED_ERR') {
                    handleerror('Session Quota exceeded!!!', e);
                }

                else {
                    handleerror('Error!!! ', e);
                }
            }
  }

  function list() {

            var sessionlist = '';
            var sessionJSON;

            sessionlist += '{"result":[';
            for (i = 0; i <= sessionStorage.length - 1; i++) {
                sessionlist += '{"key":"' + getkey(i) + '", "val":"' + getSession(getkey(i)) + '"}';
                if (i < sessionStorage.length - 1)
                    sessionlist += ',';
            }
            sessionlist += ']}';

            try {
                sessionJSON = JSON.parse(sessionlist);
            }
            catch (e) {
            }

            return sessionJSON;

  }

  function getSession(key) {

            if (key == null || key == undefined || key.toString() == '') {
                return null;
            }
            else {
                return JSON.parse(sessionStorage.getItem(key));

            }
  }

  function getkey(index) {

            if (index == null || index == undefined || index.toString() == '') {
                return null;
            }
            else {

                if (index < sessionStorage.length) {
                    return sessionStorage.key(index);
                }
                else {
                    return null;
                }
            }
  }

  function isexists(key) {
            if (getSession(key) != null || getSession(key) != undefined) {
                return true;
            }
            else {
                return false;
            }
  }

  function clear() {
            sessionStorage.clear();
  }

  function removeSession(key) {

            sessionStorage.removeItem(key);

  }
 

