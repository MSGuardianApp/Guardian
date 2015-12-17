//
//  BuddiesSelCell.h
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface BuddiesSelCell : UITableViewCell

@property (nonatomic , weak) IBOutlet UIImageView *imgprfl;
@property (nonatomic , weak) IBOutlet UILabel *lblName;
@property (nonatomic , weak) IBOutlet UILabel *lblMobileNumber;
@property (nonatomic , weak) IBOutlet UIButton *btnDelete;
@property (nonatomic , weak) IBOutlet UILabel *lblEmail;


@end
