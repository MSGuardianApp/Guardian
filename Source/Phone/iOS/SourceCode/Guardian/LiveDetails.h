//
//  LiveDetails.h
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface LiveDetails : NSObject

@property(nonatomic,strong)NSString *authenticationToken;
@property(nonatomic,strong)NSString *refreshToken;
@property(nonatomic,strong)NSString *accessToken;


@end
