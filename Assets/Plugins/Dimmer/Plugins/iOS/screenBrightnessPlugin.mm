#import <Foundation/Foundation.h>

@interface ScreenBrightness : NSObject
{
    
}

@end


@implementation ScreenBrightness

+(float)getScreenBrightness
{
    return [[UIScreen mainScreen] brightness];
}

+(void)setSreenBrightness:(float)brightness
{
    [[UIScreen mainScreen] setBrightness: brightness];
}

@end


extern "C"
{
    float getScreenBrightness()
    {
        return [ScreenBrightness getScreenBrightness];
    }
    
    void setScreenBrightness(float brightness)
    {
        [ScreenBrightness setSreenBrightness:brightness];
    }
}
