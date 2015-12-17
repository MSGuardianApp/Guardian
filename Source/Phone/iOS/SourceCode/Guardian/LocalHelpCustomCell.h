//
//  LocalHelpCustomCell.h
//  Guardian
//
//  Created by PTG on 27/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface LocalHelpCustomCell : UITableViewCell
@property (nonatomic , weak) IBOutlet UIImageView *imgprfl;
@property (nonatomic , weak) IBOutlet UIImageView *imgCall;
@property (nonatomic , retain) IBOutlet UILabel *lblName;
@property (nonatomic , retain) IBOutlet UILabel *lblAddress;
@property (nonatomic , retain) IBOutlet UIButton *btnCall;
@property (nonatomic , retain) IBOutlet UIView *viewBG;
@end
