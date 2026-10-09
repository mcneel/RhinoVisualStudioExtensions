# Builds the plug-in on Windows and macOS from one set of sources.
# Generate a project into the build folder with one of:
#   cmake -G Xcode -S . -B build
#   cmake -G "Visual Studio 18 2026" -A x64 -S . -B build
cmake_minimum_required(VERSION 3.21)

# Rhino for Mac is arm64 only.
set(CMAKE_OSX_ARCHITECTURES arm64 CACHE STRING "")
set(CMAKE_OSX_DEPLOYMENT_TARGET 15.0 CACHE STRING "")

project("MyRhino.1" LANGUAGES CXX)

set(PLUGIN_NAME "MyRhino.1")
# Target names can't contain spaces; the .rhp keeps the plug-in name.
string(REGEX REPLACE "[^A-Za-z0-9_.+-]" "_" PLUGIN_TARGET "${PLUGIN_NAME}")
set(RHINO_SDK "${CMAKE_CURRENT_SOURCE_DIR}/SDK")

if(NOT EXISTS "${RHINO_SDK}/inc/rhinoSdk.h")
  message(FATAL_ERROR "The Rhino C++ SDK is missing. Add it in this folder with:\n"
    "  git submodule add https://github.com/mcneel/rhino_sdk_cpp.git SDK")
endif()

set(SOURCES
  "MyRhino.1PlugIn.cpp"
  "cmdMyRhino__1Command.cpp"
)

if(WIN32)
  set(RHINO_EXE "C:/Program Files/Rhino 9/System/Rhino.exe" CACHE FILEPATH "Rhino to start when debugging")

  # The MFC DLL entry point and resources. Rhino for Mac does not use MFC.
  list(APPEND SOURCES "MyRhino.1App.cpp" stdafx.cpp "MyRhino.1.rc" "MyRhino.1.def")
#if (Automation)
  list(APPEND SOURCES "MyRhino.1.idl")
#endif
  # CMake's default MSVC flags define WIN32. Rhino for Windows is 64-bit only
  # and rhinoSdkChecks.h stops the build unless WIN64 is defined and WIN32 is not.
  string(REPLACE "/DWIN32" "" CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS}")
  set(CMAKE_MFC_FLAG 2) # MFC in a shared DLL
endif()

if(APPLE)
  set(RHINO_APP "/Applications/Rhino 9.app" CACHE PATH "Rhino to start when running from Xcode")
  option(RHINO_MAKE_DEBUGGABLE "Re-sign Rhino after each build so the debugger can attach" MakeRhinoDebuggableDefault)
endif()

add_library(${PLUGIN_TARGET} MODULE ${SOURCES})
set_target_properties(${PLUGIN_TARGET} PROPERTIES OUTPUT_NAME "${PLUGIN_NAME}")
target_include_directories(${PLUGIN_TARGET} PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}")
if(APPLE)
  # The SDK headers warn about their own include casing and deprecations; the Xcode
  # project silences these with the SDK's xcconfig, here they're system headers instead.
  target_include_directories(${PLUGIN_TARGET} SYSTEM PRIVATE "${RHINO_SDK}/inc")
else()
  target_include_directories(${PLUGIN_TARGET} PRIVATE "${RHINO_SDK}/inc")
endif()

if(WIN32)
  # Rhino plug-ins always link the release CRT and MFC, even when built with
  # debugging information. A plug-in built against the debug CRT will not load,
  # so RHINO_DEBUG_PLUGIN takes the place of _DEBUG.
  set_target_properties(${PLUGIN_TARGET} PROPERTIES
    CXX_STANDARD 20 CXX_STANDARD_REQUIRED ON
    MSVC_RUNTIME_LIBRARY "MultiThreadedDLL" SUFFIX ".rhp"
    VS_DEBUGGER_COMMAND "${RHINO_EXE}")
  set_property(DIRECTORY PROPERTY VS_STARTUP_PROJECT ${PLUGIN_TARGET})
  target_compile_definitions(${PLUGIN_TARGET} PRIVATE
    $<$<CONFIG:Debug>:RHINO_DEBUG_PLUGIN> $<$<NOT:$<CONFIG:Debug>>:NDEBUG>)
  # RHINO_LIB_DIR is pasted into the SDK's #pragma comment(lib, ...) lines, which
  # the linker resolves relative to the build directory, so it has to be absolute.
  target_compile_definitions(${PLUGIN_TARGET} PRIVATE
    WIN64 _WINDOWS _USRDLL _AFXDLL _UNICODE UNICODE
    RHINO_LIB_DIR="${RHINO_SDK}/lib")
  # stdafx.h has to be compiled first in every source file.
  target_compile_options(${PLUGIN_TARGET} PRIVATE "/FI${CMAKE_CURRENT_SOURCE_DIR}/stdafx.h" /W4)
#if (SDL)
  target_compile_options(${PLUGIN_TARGET} PRIVATE /sdl)
#endif
#if (Automation)
  # The resources embed the type library MIDL writes to the intermediate folder.
  target_include_directories(${PLUGIN_TARGET} PRIVATE "${CMAKE_CURRENT_BINARY_DIR}/${PLUGIN_TARGET}.dir/$<CONFIG>")
#endif
endif()

if(APPLE)
  # stdafx.h has to be compiled first in every source file.
  target_compile_options(${PLUGIN_TARGET} PRIVATE
    "SHELL:-include \"${CMAKE_CURRENT_SOURCE_DIR}/stdafx.h\""
    -x objective-c++ -fobjc-arc -fno-operator-names)
  target_compile_definitions(${PLUGIN_TARGET} PRIVATE
    ON_COMPILER_CLANG ON_RUNTIME_APPLE RHINO_APPLE=1 _GNU_SOURCE MY_ZCALLOC
    Z_PREFIX _UNICODE RHINO_V6_READY RHINO_THIRD_PARTY_OSX_PLUGIN_COMPILE
    $<$<CONFIG:Debug>:_DEBUG=1 ON__DEBUG> $<$<NOT:$<CONFIG:Debug>>:NDEBUG=1>)
  # ARC needs the Objective-C runtime.
  target_link_options(${PLUGIN_TARGET} PRIVATE -fobjc-link-runtime)
  # The real frameworks are found inside Rhino at run time.
  target_link_libraries(${PLUGIN_TARGET} PRIVATE
    "${RHINO_SDK}/lib/RhCore.tbd"
    "${RHINO_SDK}/lib/OpenNURBS.tbd"
    "${RHINO_SDK}/lib/RhMaterialEditor.tbd")

  string(REGEX REPLACE "[^A-Za-z0-9.-]" "-" BUNDLE_ID_NAME "${PLUGIN_NAME}")
  # A plug-in is a loadable bundle, so the package type has to be BNDL;
  # CMake's default plist says APPL and Rhino will not load that.
  set_target_properties(${PLUGIN_TARGET} PROPERTIES
    CXX_STANDARD 14 CXX_STANDARD_REQUIRED ON
    BUNDLE TRUE BUNDLE_EXTENSION rhp
    MACOSX_BUNDLE_INFO_PLIST "${CMAKE_CURRENT_SOURCE_DIR}/Info.plist.in"
    MACOSX_BUNDLE_EXECUTABLE_NAME "${PLUGIN_NAME}"
    MACOSX_BUNDLE_BUNDLE_NAME "${PLUGIN_NAME}"
    MACOSX_BUNDLE_GUI_IDENTIFIER "com.mycompany.${BUNDLE_ID_NAME}"
    MACOSX_BUNDLE_SHORT_VERSION_STRING 1.0
    MACOSX_BUNDLE_BUNDLE_VERSION 1
    XCODE_ATTRIBUTE_PRODUCT_BUNDLE_IDENTIFIER "com.mycompany.${BUNDLE_ID_NAME}"
    XCODE_ATTRIBUTE_CODE_SIGN_IDENTITY "-"
    # Run starts Rhino, which loads the plug-in from the build folder.
    XCODE_GENERATE_SCHEME ON
    XCODE_SCHEME_EXECUTABLE "${RHINO_APP}"
    XCODE_SCHEME_ENVIRONMENT "RHINO_PACKAGE_DIRS=$(BUILT_PRODUCTS_DIR)")

  # Rhino is not debuggable by default; this re-signs it once so the debugger can attach.
  if(RHINO_MAKE_DEBUGGABLE)
    add_custom_command(TARGET ${PLUGIN_TARGET} POST_BUILD
      COMMAND "${RHINO_SDK}/script/make_rhino_debuggable.sh" "${RHINO_APP}"
      COMMENT "Making Rhino debuggable"
      VERBATIM)
  endif()
endif()
