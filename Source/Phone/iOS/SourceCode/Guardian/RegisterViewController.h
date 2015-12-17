//
//  RegisterViewController.h
//  Guardian
//
//  Created by PTG on 18/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "countryXml.h"
@class TPKeyboardAvoidingScrollView;

@interface RegisterViewController : UIViewController<UIPickerViewDelegate,UIPickerViewDataSource,UIActionSheetDelegate>{
    NSString *strPhoneNumber;
    UIPickerView *regionPicker;
    NSMutableArray *arrCountryList;
    UIActionSheet *Region_Actionsheet;
    UIAlertController* regionPickerContainer;
    NSInteger selIndex;
    NSString *prevString;
    NSInteger maxPhoneDigit;
    NSString *prevRegCode;
    BOOL isKeyboardShown;
    BOOL isEdited;
}
@property (nonatomic , assign)BOOL isEdit;
@property (nonatomic , retain)NSString *Phonetxt;
@end
