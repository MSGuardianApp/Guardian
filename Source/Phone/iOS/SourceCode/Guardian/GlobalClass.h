//
//  GlobalClass.h
//  OOW
//
//  Created by PTG on 26/08/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol LocationPostProtocol <NSObject>


@optional
-(void)postedSuccessfully:(NSString *)successStr;
-(void)stopPostedSuccessfully:(NSString *)successStr;
-(void)PostedWithMediaSuccessfully:(NSString *)successStr;
@end

@interface GlobalClass : NSObject{
    NSInteger SOSCallCount;
}

@property(nonatomic,retain)id <LocationPostProtocol> PostProtocol;
+ (GlobalClass *)sharedInstance;
- (BOOL) emailValidation:(NSString *)email;
- (NSString*)base64forData:(NSData*)theData;
- (BOOL)connected;
- (BOOL) is468SizeScreen;
-(NSInteger)checkInternetStatus;
- (NSString *) networkChecking;
-(void)stopPostingandIndex:(NSInteger)NoOfTimes;
-(void)stopSOSandIndex:(NSInteger)NoOfTimes;
-(void)insertProfileDataToDB:(NSMutableDictionary *)dict ;
-(NSString *) dateToTicks:(NSDate *) date;
-(void)postLocations:(NSMutableArray *)arr andIndex:(NSInteger)NoOfTimes;
-(void)postLocationsWithMedia:(NSMutableDictionary *)dict andIndex:(NSInteger)NoOfTimes;
-(NSString *)getSessiontoken;
-(void)postLocationWithMediaContent:(NSMutableArray *)byteArray;
- (BOOL)isFullScreen;
-(void)migrationUpdate;
-(void)saveExceptionText:(NSString *)txt;
-(void)postMsgtoFB:(NSString *)msg andIndex:(NSInteger)NoOfTimes;
- (UIImage*)imageWithImage:(UIImage*)image scaledToSize:(CGSize)newSize;
- (NSString *) GetUTCDateTimeFromLocalTime:(NSString *)IN_strLocalTime;
-(NSString *) ticksToDate:(NSString *) ticks;
-(NSString *) utcdateToTicks:(NSDate *) date;
@end
