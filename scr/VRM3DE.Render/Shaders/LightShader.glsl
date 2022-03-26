#version 330 core

uniform vec2 CameraResolution;
uniform vec3 CameraPosition;
uniform vec2 CameraRotation;

const float FOV = 1.5555;
const float MaxDist = 500.0;
const float Epsilon = 0.001;
const int MaxSteps = 256;

struct object
{
	float ID;
	vec3 color;
};

struct hit
{
	vec3 point;
	float dist;
	object object;
};

mat2 rotation(float a)
{
	float sin = sin(a);
	float cos = cos(a);
	return mat2(cos, -sin, sin, cos);
}


float sphereDist(vec3 point, float radius)
{
	return length(point) - radius;
}

float planeDist(vec3 point, vec3 normal, float distanceFromOrigin)
{
	return dot(point, normal) + distanceFromOrigin;
}

float torusDist(vec3 point, float smallRadius, float largeRadius)
{
	return length(vec2(length(point.xz) - largeRadius, point.y)) - smallRadius;
}

hit unite(hit first, hit second)
{
	return (first.dist < second.dist) ? first : second;
}

hit closestMapHit(vec3 point)
{
	object plane = object(0.0, vec3(0.2 + 0.4 * mod(floor(point.x) - floor(point.z), 2.0)));
	hit planeHit = hit(point, planeDist(point, vec3(0.0, 1.0, 0.0), 1.0), plane);
	object sphere = object(1.0, vec3(0.9, 0.0, 0.0));
	hit sphereHit = hit(point, sphereDist(point, 1.0), sphere);
	hit result = unite(planeHit, sphereHit);
	return result;
}


void createRay(inout vec3 origin, inout vec3 direction)
{
	vec2 UV = vec2(2.0 * (gl_FragCoord.xy) - CameraResolution.xy) / CameraResolution.y;
	origin = CameraPosition.zyx;
	direction = normalize(vec3(UV, FOV));
	direction.zy *= rotation(-CameraRotation.y);
	direction.zx *= rotation(CameraRotation.x);
}

void traceRay(inout vec3 origin, inout vec3 direction, inout vec3 end, inout float lengths, inout hit hit)
{
	for (int i = 0; i < MaxSteps; i++)
	{
		end = origin + lengths * direction;
		hit = closestMapHit(end);
		lengths += hit.dist;
		if (abs(hit.dist) < Epsilon || lengths >= MaxDist)
		{
			break;
		}
	}
}

vec3 shade(vec3 origin, vec3 direction, vec3 end, float lengths)
{
    vec3 lightPosition = vec3(20.0, 40.0, -30.0);
	float S = 0.05;
    vec3 L = normalize(lightPosition - end);
	vec2 E = vec2(Epsilon, -0.0);
	vec3 N = normalize(vec3(closestMapHit(end).dist) - vec3(closestMapHit(end - E.xyy).dist, closestMapHit(end - E.yxy).dist, closestMapHit(end - E.yyx).dist));
	vec3 diffuse = vec3(1.0) * clamp(dot(L, N), S, 1.0);
	vec3 lightOrigin = end + N * 0.02;
	vec3 lightDirection = normalize(lightPosition);
	vec3 lightEnd;
	float lightLengths;
	hit lightHit;
	traceRay(lightOrigin, lightDirection, lightEnd, lightLengths, lightHit);
	if (lightLengths < length(lightPosition - end))
	{
		return vec3(S);
	}
	return diffuse;
}

vec3 renderRay(vec3 origin, vec3 direction, vec3 end, float lengths, hit hit)
{
	vec3 color;
	vec3 sky = vec3(0.5, 0.8, 0.9);
	if (lengths >= MaxDist)
	{
		color = sky - max(0.75 * direction.y, 0.0);
	}
	else
	{
		color += hit.object.color;
		color *= shade(origin, direction, end, lengths);
		color = mix(color, sky, 1.0 - exp(-0.00005 * lengths * lengths));
	}
	return color;
}


void main()
{
	vec3 color;
	vec3 origin;
	vec3 direction;
	vec3 end;
	float lengths;
	hit hit;
	createRay(origin, direction);
	end = origin;
	traceRay(origin, direction, end, lengths, hit);
	color += renderRay(origin, direction, end, lengths, hit);
	color = pow(color, vec3(0.4545));
	gl_FragColor = vec4(color, 1.0);
}
