package com.guardianapp.views;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.CheckBox;

import com.guardianapp.R;

public class CusCheckBox extends CheckBox{



    public CusCheckBox(Context context, AttributeSet attrs) {
            super(context, attrs);
            //setButtonDrawable(new StateListDrawable());
        }
        @Override
        public void setChecked(boolean t){
            if(t)
            {
                this.setButtonDrawable(R.drawable.check_yes);
            }
            else
            {
                this.setButtonDrawable(R.drawable.check_no);
            }
            super.setChecked(t);
        }

}