//
//  GroupCustomcell.h
//  Guardian
//
//  Created by PTG on 25/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface GroupCustomcell : UITableViewCell
@property (nonatomic , weak) IBOutlet UILabel *lblEmail;
@property (nonatomic , weak) IBOutlet UILabel *lblName;
@property (nonatomic , weak) IBOutlet UILabel *lblMobileNumber;
@property (nonatomic , weak) IBOutlet UIButton *btnDelete;
@end
