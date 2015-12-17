//
//  MicrosoftLiveConnect.h
//  Guardian
//
//  Created by PTG on 18/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "LiveConnectClient.h"


@interface MicrosoftLiveConnect : UIViewController <LiveAuthDelegate,LiveOperationDelegate>{
    BOOL isOldProfile;
}

@property (nonatomic, retain) LiveConnectClient *liveClient;

@end