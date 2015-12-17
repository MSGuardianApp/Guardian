//
//  LocateCustomCell.h
//  Guardian
//
//  Created by PTG on 22/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface LocateCustomCell : UITableViewCell
@property (nonatomic , retain) IBOutlet UILabel *lblName;
@property (nonatomic , retain) IBOutlet UILabel *lblPhoneNumber;
@property (nonatomic , retain) IBOutlet UILabel *lblAddress;
@property (nonatomic , retain) IBOutlet UIButton *btnMap;
@property (nonatomic , retain) IBOutlet UIButton *btnCall;
@property (nonatomic , retain) IBOutlet UIButton *btnSMS;
@property (nonatomic , retain) IBOutlet UIButton *btnMail;
@property (nonatomic , retain) IBOutlet UIView *viewBG;
@end
