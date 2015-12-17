//
//  PreferencesViewController.h
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "countryXml.h"
#import <CoreLocation/CoreLocation.h>

@interface PreferencesViewController : UIViewController<UIScrollViewDelegate,UIPickerViewDelegate,UIPickerViewDataSource,UITextFieldDelegate,UIActionSheetDelegate>{
    NSMutableArray *arrCountryList;
    NSMutableArray *arrProfileInfo;
    NSMutableArray *arrBuddy;
    NSMutableArray *arrFBGroups;
    UIPickerView *regionPicker;
    UIActionSheet *Region_Actionsheet;
    UIAlertController* regionPickerContainer;
    NSString *prevString;
    NSInteger maxPhoneDigit;
    NSString *prevRegCode;
    NSString *countryName;
    NSInteger selIndex;
}

@end
