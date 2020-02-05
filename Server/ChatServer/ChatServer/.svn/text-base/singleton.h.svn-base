#ifndef SINGLETON_SINGLETON_H
#define SINGLETON_SINGLETON_H

template<class T>
class Singleton {
public:
	static void Create() {
		if(s_pInstance == NULL)
		{
			s_pInstance = new T();
		}
	}

	static void Release() {
		delete s_pInstance;
		s_pInstance = NULL;
	}

	static T* getInstance() {
		return s_pInstance;
	}

protected:
	Singleton() {
		s_pInstance = NULL;
	};

	static T *s_pInstance;
};

template<class T>
T* Singleton<T>::s_pInstance = NULL;

#endif //SINGLETON_SINGLETON_H