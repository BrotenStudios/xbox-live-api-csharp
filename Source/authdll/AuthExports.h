#pragma once

#include <stdint.h>

#if defined(SIDECAR_AUTH_EXPORTS)
#define SIDECAR_AUTH_API __declspec(dllexport)
#else
#define SIDECAR_AUTH_API __declspec(dllimport)
#endif

extern "C"
{
#pragma pack(push, 8)
	struct SignInResult
	{
		int32_t status;
	};

	struct TokenAndSignatureResult 
	{
		wchar_t * token;
		wchar_t * signature;
		wchar_t * xboxUserId;
		wchar_t * gamerTag;
		wchar_t * xboxUserHash;
		wchar_t * ageGroup;
		wchar_t * privileges;
		wchar_t * webAccountId;
		wchar_t * reserved;
	};
#pragma pack(pop)

	typedef void(__stdcall *SignInCompletedCallback)(int32_t xblErrorCode, SignInResult result);
	SIDECAR_AUTH_API void SignIn(void *coreDispatcher, bool showUI, SignInCompletedCallback onCompleted);

	typedef void(__stdcall *GetTokenAndSignatureCallback)(int32_t xblErrorCode, TokenAndSignatureResult result);
	SIDECAR_AUTH_API void GetTokenAndSignature(wchar_t* httpMethod, wchar_t* url, wchar_t* headers, wchar_t* body, GetTokenAndSignatureCallback callback);
}
