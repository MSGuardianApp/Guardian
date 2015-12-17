//
//  GeoTag.m
//  Guardian
//
//  Created by PTG on 04/03/15.
//  Copyright (c) 2015 People Tech Group. All rights reserved.
//

#import "GeoTag.h"

@implementation GeoTag

- (void)encodeWithCoder:(NSCoder *)encoder
{
    [encoder encodeObject:self.Lati forKey:@"Lati"];
    [encoder encodeObject:self.Longi forKey:@"Longi"];
    [encoder encodeObject:self.status forKey:@"status"];
    [encoder encodeObject:self.Speed forKey:@"Speed"];
    [encoder encodeObject:self.Altitude forKey:@"Altitude"];
    [encoder encodeObject:self.timeStamp forKey:@"timeStamp"];
    [encoder encodeObject:self.accuracy forKey:@"accuracy"];
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if(self = [super init])
    {
        self.Lati = [aDecoder decodeObjectForKey:@"Lati"];
        self.Longi = [aDecoder decodeObjectForKey:@"Longi"];
        self.status = [aDecoder decodeObjectForKey:@"status"];
        self.Speed = [aDecoder decodeObjectForKey:@"Speed"];
        self.Altitude = [aDecoder decodeObjectForKey:@"Altitude"];
        self.timeStamp = [aDecoder decodeObjectForKey:@"timeStamp"];
        self.accuracy = [aDecoder decodeObjectForKey:@"accuracy"];
    }
    return self;
}
@end
