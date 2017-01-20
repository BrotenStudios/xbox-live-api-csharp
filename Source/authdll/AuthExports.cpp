#include "AuthExports.h"

#define XSAPI_CPP 1
#define _NO_XSAPIIMP
#define WINAPI_FAMILY WINAPI_FAMILY_APP
#include <xsapi/services.h>
#include <roapi.h>

using namespace xbox::services;
using namespace xbox::services::system;

namespace
{
	std::vector<std::shared_ptr<xbox_live_user>> AllUsers;
	std::shared_ptr<xbox_live_user> SingleUser;
}

extern "C" void SignIn(void *coreDispatcher, bool showUI, SignInCompletedCallback onCompleted)
{
    RoInitialize(RO_INIT_MULTITHREADED);
	SingleUser = std::make_shared<xbox_live_user>();
	Platform::Object^ dispatcherRT = reinterpret_cast<Platform::Object^>(static_cast<IInspectable*>(coreDispatcher));
	SingleUser->signin(dispatcherRT).then(
		[onCompleted](xbox_live_result<sign_in_result> xblResult)
	{
		SignInResult result;
		result.status = xblResult.payload().status();
		onCompleted(xblResult.err().value(), result);
	});
}

extern "C" void GetTokenAndSignature(wchar_t* httpMethod, wchar_t* url, wchar_t* headers, wchar_t* body, GetTokenAndSignatureCallback callback)
{
	SingleUser->get_token_and_signature(httpMethod, url, headers, body).then(
		[callback](xbox_live_result<token_and_signature_result> xblResult)
	{
		TokenAndSignatureResult result;
		auto payload = xblResult.payload();
		result.token = const_cast<wchar_t*>(payload.token().c_str());
		result.ageGroup = const_cast<wchar_t*>(payload.age_group().c_str());
		result.gamerTag = const_cast<wchar_t*>(payload.gamertag().c_str());
		result.privileges = const_cast<wchar_t*>(payload.privileges().c_str());
		result.reserved = const_cast<wchar_t*>(payload.reserved().c_str());
		result.signature = const_cast<wchar_t*>(payload.signature().c_str());
		result.webAccountId = const_cast<wchar_t*>(payload.web_account_id().c_str());
		result.xboxUserHash = const_cast<wchar_t*>(payload.xbox_user_hash().c_str());
		result.xboxUserId = const_cast<wchar_t*>(payload.xbox_user_id().c_str());

		callback(xblResult.err().value(), result);
	});
}
