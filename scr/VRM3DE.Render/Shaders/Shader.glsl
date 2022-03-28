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
	float specularity;
};

struct hit
{
	vec3 point;
	float dist;
	object object;
};

struct ray
{
	vec3 origin;
	vec3 direction;
	vec3 end;
	float lengths;
	hit hit;
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
	object plane = object(0.0, vec3(0.2 + 0.4 * mod(floor(point.x) - floor(point.z), 2.0)), 1.0);
	hit planeHit = hit(point, planeDist(point, vec3(0.0, 1.0, 0.0), 1.0), plane);
	object sphere = object(1.0, vec3(0.9, 0.0, 0.0), 1.0);
	hit sphereHit = hit(point, sphereDist(point, 1.0), sphere);
	hit result = unite(planeHit, sphereHit);
	return result;
}


ray createRay(vec2 offset)
{
	vec2 UV = vec2(2.0 * (gl_FragCoord.xy + offset) - CameraResolution.xy) / CameraResolution.y;
	hit hit = hit(vec3(0.0), 0.0, object(0.0, vec3(0.0), 0.0));
	ray ray = ray(CameraPosition.zyx, normalize(vec3(UV, FOV)), CameraPosition.zyx, 0.0, hit);
	ray.direction.zy *= rotation(-CameraRotation.y);
	ray.direction.zx *= rotation(CameraRotation.x);
	return ray;
}

ray traceRay(ray orig)
{
	ray ray = orig;
	for (int i = 0; i < MaxSteps; i++)
	{
		ray.end = ray.origin + ray.lengths * ray.direction;
		ray.hit = closestMapHit(ray.end);
		ray.lengths += ray.hit.dist;
		if (abs(ray.hit.dist) < Epsilon || ray.lengths >= MaxDist)
		{
			break;
		}
	}
	return ray;
}

vec3 shade(ray ray, float specularity)
{
    vec3 lightPosition = vec3(20.0, 40.0, -30.0);
    float S = 0.05;
    vec3 L = normalize(lightPosition - ray.end);
	vec2 E = vec2(Epsilon, 0.0);
	vec3 N = normalize(vec3(closestMapHit(ray.end).dist) - vec3(closestMapHit(ray.end - E.xyy).dist, closestMapHit(ray.end - E.yxy).dist, closestMapHit(ray.end - E.yyx).dist));
	vec3 V = -ray.direction;
	vec3 R = reflect(-L, N);
	vec3 diffuse = vec3(clamp(dot(L, N), 0.0, 1.0));
	vec3 specular = vec3(0.5) * pow(clamp(dot(R, V), 0.0, specularity), 32.0);
    vec3 ambient = vec3(S);
	hit hit = hit(vec3(0.0), 0.0, struct object(0.0, vec3(0.0), 0.0));
	struct ray lightRay = struct ray(ray.end + N * 0.02, normalize(lightPosition), vec3(0.0), 0.0, hit);
	lightRay = traceRay(lightRay);
	if (lightRay.lengths < length(lightPosition - ray.end))
	{
		return ambient;
	}
	return diffuse + ambient + specular;
}

vec3 renderRay(ray ray)
{
	vec3 color = vec3(0.0);
	vec3 sky = vec3(0.5, 0.8, 0.9);
	if (ray.lengths >= MaxDist)
	{
		color = sky - max(0.75 * ray.direction.y, 0.0);
	}
	else
	{
	    color += ray.hit.object.color;
		color *= shade(ray, ray.hit.object.specularity);
		color = mix(color, sky, 1.0 - exp(-0.00005 * ray.lengths * ray.lengths));
	}
	return color;
}


void main()
{
	vec2[] offset = vec2[](vec2(0.125, 0.375), vec2(0.375, -0.125), vec2(-0.125, -0.375), vec2(-0.375, 0.125));
	vec3 color;
	for (int i = 0; i < offset.length; i++)
	{
		ray ray = createRay(offset[i]);
		ray = traceRay(ray);
		color += renderRay(ray);
	}
	color = pow(color / offset.length, vec3(0.4545));
	gl_FragColor = vec4(color, 1.0);
}
