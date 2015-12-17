package com.guardianapp.ui;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.support.v4.view.ViewPager;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TextView;

import com.guardianapp.R;
import com.guardianapp.adapters.HelpPageAdapter;


@SuppressLint("NewApi")
public class HelpActivity extends BaseActivity {

	private int[] Images = new int[] { R.drawable.h1, R.drawable.h2,
			R.drawable.h3, R.drawable.h4,R.drawable.h5,R.drawable.h6,R.drawable.h7,R.drawable.h8,R.drawable.h9,R.drawable.h10
			,R.drawable.h11,R.drawable.h12,R.drawable.h13,R.drawable.h14,R.drawable.h15,R.drawable.h16,R.drawable.h17,R.drawable.h18
			,R.drawable.h19,R.drawable.h20,R.drawable.h21,R.drawable.h22,R.drawable.h23,R.drawable.h24,R.drawable.h25,R.drawable.h26
	};


	private ViewPager mViewPager;
	private TextView countTv;
	@SuppressLint("NewApi") @Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getSupportActionBar().setTitle("HELP"); 
		setContentView(R.layout.help_screen);
	}
	
	@Override
	protected void initFields() {
		// TODO Auto-generated method stub
		mViewPager = (ViewPager) findViewById(R.id.helpFragmentLayout);
		mViewPager.setOnPageChangeListener(onPageChangeListener);
		mViewPager.setAdapter(new HelpPageAdapter(getSupportFragmentManager(),Images));
		countTv = (TextView) findViewById(R.id.textcount);
		countTv.setText("1"+"/"+Images.length);
	}

	@Override
	protected void initCallBacks() {
		// TODO Auto-generated method stub
		findViewById(R.id.imageBack).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				mViewPager.setCurrentItem(mViewPager.getCurrentItem()-1);

			}
		});

		findViewById(R.id.imageNext).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				mViewPager.setCurrentItem(mViewPager.getCurrentItem()+1);
			}
		});
		
	}

	private ViewPager.SimpleOnPageChangeListener onPageChangeListener = new ViewPager.SimpleOnPageChangeListener() {
		@Override
		public void onPageSelected(int position) {
			super.onPageSelected(position);
			position = position+1;
			String count = ""+position+"/"+Images.length;
			countTv.setText(count);
		}
	};


}