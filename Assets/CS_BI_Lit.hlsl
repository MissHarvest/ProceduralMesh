void Ripple_float(
	float3 PositionIn, float3 Origin,
	float Period, float Speed, float Amplitude,
	out float3 PositionOut, out float3 NormalOut
)
{
    float3 p = PositionIn - Origin;
    float d = length(p);
    float f = 2.0 * PI * Period * (d - Speed * _Time.y);
	
    PositionOut = PositionIn + float3(0.0, 0.0, Amplitude * sin(f));
    
    float2 derivatives = (2.0 * PI * Amplitude * Period * cos(f) / max(d, 0.0001)) * p.xz;
    
    NormalOut = cross(float3(0.0, 1.0, derivatives.y), float3(1.0, 0.0, derivatives.x));
}